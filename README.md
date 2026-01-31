# Aves pipeline

Bird taxonomy and audio data processing pipeline

## Disclaimer

This project is an **educational data processing pipeline** created as part of a university course.
It is not intended for production use.

The focus of this project was to design and implement a multi-step pipeline that:

- integrates multiple external systems
- processes heterogeneous data sources
- applies transformations and aggregation
- produces a reproducible final dataset

Some parts are intentionally simplified or incomplete to keep the scope reasonable for a semester project.

## Project Overview

Aves Pipeline is a .NET Worker Service that processes bird-related data through a series of well-defined pipeline steps.

The pipeline combines:

- scraped taxonomy data
- audio recordings of bird calls
- machine-learning classification results
- produces a CSV dataset containing aggregated bird observation statistics

The solution follows a pipeline-oriented architecture, where each step is isolated, testable, and executed sequentially from a single entry point.

## Pipeline Steps

The pipeline is composed of the following steps:

### 1. Taxonomy Step

- Scrapes bird taxonomy data from a mock ornithological website (`https://aves.regoch.net`, based on GBIF data)
- Stores taxonomy data in MongoDB
- Skips scraping if taxonomy data already exists

Purpose:

- Establish a taxonomy baseline (order, family, genus, scientific name)
- Ensure idempotent execution

### 2. Audio Processing Step

- Reads audio files from a configured input directory
- Uploads audio files to S3-compatible object storage
- Sends audio to a public bird-call classification API
- Stores audio object keys and classification results
- Persists all metadata in MongoDB

Purpose:

- Link raw audio data with classification results
- Enable later analysis and aggregation

### 3. Statistics Step

- Loads all stored audio classifications and taxonomy data
- Cleans and flattens classification results
- Applies:
  - minimum confidence threshold
  - optional fuzzy species name filter (regex)
- Aggregates statistics per species
- Exports results as a CSV file
- Generated statistics include:
  - scientific name
  - common name
  - observation count
  - average / min / max confidence
  - taxonomic hierarchy (order, family, genus)

## Features

- Modular pipeline architecture
- MongoDB persistence
- S3-compatible object storage support
- Audio classification integration
- CSV export
- Configurable execution parameters
- Dockerized infrastructure

## Known Limitations

This project intentionally does not aim to be a production-grade data pipeline.

Notable limitations include:

- No retry or backoff logic for external services
- No distributed execution
- No parallel step execution
- No schema evolution handling
- No deduplication beyond simple existence checks
- Naive taxonomy matching logic
- Limited error recovery

These tradeoffs were made to focus on pipeline design and integration, not robustness at scale.

## Why Build This Pipeline?

The purpose of this project was learning, not building a commercial system.

By implementing this pipeline, the project explores:

- how real-world data pipelines are structured
- how to orchestrate dependent processing steps
- how to integrate databases, object storage, and HTTP APIs
- why data cleaning and aggregation are non-trivial
- how configuration impacts reproducibility

## Technologies Used

- .NET 8 (Worker Service)
- MongoDB
- S3-compatible object storage (SeaweedFS)
- Docker & Docker Compose

## Setup

### Requirements

- .NET 8.0 SDK
- Docker & Docker Compose

### Infrastructure (Docker)

Start MongoDB and S3-compatible storage:

```bash
docker compose up -d mongo seaweedfs
```

### Configuration

Pipeline behavior is controlled via configuration and environment variables, including:

- confidence threshold
- species name filter (regex)
- input/output directories
- geographic coordinates

### Running the Pipeline

Run locally:

```bash
dotnet run --project AvesPipeline.WorkerService
```

Run via Docker:

```bash
docker compose up --build
```
