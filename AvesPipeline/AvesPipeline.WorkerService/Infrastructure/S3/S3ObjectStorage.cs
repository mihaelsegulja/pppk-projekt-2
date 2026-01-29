using Amazon.S3;
using Amazon.S3.Model;
using AvesPipeline.WorkerService.Options;
using Microsoft.Extensions.Options;

namespace AvesPipeline.WorkerService.Infrastructure.S3;

public sealed class S3ObjectStorage : IS3ObjectStorage
{
    private readonly AmazonS3Client _s3;
    private readonly S3Options _options;

    public S3ObjectStorage(IOptions<S3Options> options)
    {
        _options = options.Value;

        var config = new AmazonS3Config
        {
            ServiceURL = _options.BaseUrl,
            ForcePathStyle = true,
            AuthenticationRegion = "us-east-1",
        };

        _s3 = new AmazonS3Client(new Amazon.Runtime.AnonymousAWSCredentials(), config);
        
        try
        {
            _s3.PutBucketAsync(new PutBucketRequest
            {
                BucketName = _options.Bucket
            }).GetAwaiter().GetResult();
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            // Bucket already exists, ignore
        }
    }

    public async Task<S3ObjectRef> UploadAsync(
        Stream content,
        string key,
        string contentType,
        IDictionary<string, string>? metadata,
        CancellationToken ct)
    {
        var request = new PutObjectRequest
        {
            BucketName = _options.Bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType
        };

        if (metadata != null)
        {
            foreach (var (k, v) in metadata)
                request.Metadata[k] = v;
        }

        await _s3.PutObjectAsync(request, ct);

        return new S3ObjectRef
        {
            Bucket = _options.Bucket,
            Key = key,
            ContentType = contentType
        };
    }

    public async Task<Stream> DownloadAsync(string key, CancellationToken ct)
    {
        var response = await _s3.GetObjectAsync(
            _options.Bucket,
            key,
            ct);

        return response.ResponseStream;
    }
}