using AngleSharp;
using AngleSharp.Dom;
using Microsoft.Playwright;
using AvesPipeline.WorkerService.Models;
using System.Collections.Concurrent;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;

namespace AvesPipeline.WorkerService.Infrastructure.Web;

public sealed class TaxonomyScraper : ITaxonomyScraper, IAsyncDisposable
{
    private readonly ILogger<TaxonomyScraper> _logger;
    private readonly TaxonomyScraperOptions _options;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _indexPage;

    public TaxonomyScraper(ILogger<TaxonomyScraper> logger, IOptions<TaxonomyScraperOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Args = new[] { "--disable-gpu", "--no-sandbox", "--disable-extensions" }
        });

        var context = await _browser.NewContextAsync();
        _indexPage = await context.NewPageAsync();
    }

    public async Task<IReadOnlyList<TaxonDto>> ScrapeAsync(CancellationToken ct)
    {
        if (_indexPage is null)
            await InitializeAsync();

        await _indexPage!.GotoAsync(_options.BaseUrl);
        await _indexPage.WaitForSelectorAsync("#speciesTable tbody tr");

        var pageInfo = await _indexPage.InnerTextAsync("#pageInfo");
        var totalPages = int.TryParse(pageInfo.Split("of").LastOrDefault()?.Trim(), out var tp) ? tp : 1;
        _logger.LogInformation("Total pages detected: {TotalPages}", totalPages);

        var allResults = new List<TaxonDto>();

        for (int page = 1; page <= totalPages; page++)
        {
            ct.ThrowIfCancellationRequested();
            _logger.LogInformation("Scraping index page {Page} of {TotalPages}", page, totalPages);

            var rows = await _indexPage.QuerySelectorAllAsync("#speciesTable tbody tr");
            var resultsBag = new ConcurrentBag<TaxonDto>();

            var semaphore = new SemaphoreSlim(_options.MaxParallelDetails);
            var tasks = rows.Select((row, i) => Task.Run(async () =>
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    var cells = await row.QuerySelectorAllAsync("td");
                    if (cells.Count < 5) return;

                    var link = await cells[0].QuerySelectorAsync("a");
                    if (link == null) return;

                    var href = await link.GetAttributeAsync("href");
                    if (href == null || !href.Contains("id=")) return;

                    var gbifId = int.Parse(href.Split("id=")[1]);

                    var indexData = new
                    {
                        GbifId = gbifId,
                        ScientificName = (await cells[0].InnerTextAsync()).Trim(),
                        CanonicalName = (await cells[1].InnerTextAsync()).Trim(),
                        Rank = (await cells[2].InnerTextAsync()).Trim(),
                        Family = (await cells[3].InnerTextAsync()).Trim(),
                        Order = (await cells[4].InnerTextAsync()).Trim()
                    };

                    _logger.LogInformation("Fetching details for {ScientificName} (GBIF ID: {GbifId}) [{Row}/{TotalRows}]",
                        indexData.ScientificName, indexData.GbifId, i + 1, rows.Count);

                    var details = await FetchDetailsAsync(indexData.GbifId, ct);

                    resultsBag.Add(new TaxonDto
                    {
                        GbifId = indexData.GbifId,
                        ScientificName = indexData.ScientificName,
                        CanonicalName = indexData.CanonicalName,
                        Rank = indexData.Rank,
                        Family = indexData.Family,
                        Order = indexData.Order,
                        Kingdom = details.Kingdom,
                        Phylum = details.Phylum,
                        Class = details.Class,
                        Genus = details.Genus
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            }, ct)).ToArray();

            await Task.WhenAll(tasks);
            allResults.AddRange(resultsBag);

            if (page < totalPages)
            {
                await _indexPage.ClickAsync("button:has-text(\"Next\")");
                await _indexPage.WaitForFunctionAsync(
                    "() => document.querySelectorAll('#speciesTable tbody tr').length > 0",
                    new PageWaitForFunctionOptions { Timeout = 5000 }
                );
            }
        }

        return allResults;
    }

    private async Task<(string Kingdom, string Phylum, string Class, string Genus)> FetchDetailsAsync(int gbifId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (_browser is null)
            throw new InvalidOperationException("Browser not initialized");

        var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        try
        {
            await page.GotoAsync($"{_options.BaseUrl}/details.html?id={gbifId}");
            await page.WaitForSelectorAsync("#details dd", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Attached
            });

            var html = await page.ContentAsync();

            var document = await BrowsingContext
                .New(Configuration.Default)
                .OpenAsync(req => req.Content(html), ct);

            var dd = document.QuerySelectorAll("#details dd");

            return (
                Kingdom: dd.ElementAtOrDefault(3)?.TextContent.Trim() ?? "",
                Phylum: dd.ElementAtOrDefault(4)?.TextContent.Trim() ?? "",
                Class: dd.ElementAtOrDefault(5)?.TextContent.Trim() ?? "",
                Genus: dd.ElementAtOrDefault(8)?.TextContent.Trim() ?? ""
            );
        }
        finally
        {
            await page.CloseAsync();
            await context.CloseAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }
}
