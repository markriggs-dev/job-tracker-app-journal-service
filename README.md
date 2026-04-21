# job-tracker-app-journal-service

Manages activity journal entries for job requisitions including raw message capture and contact tagging.

## Technology
- .NET 8 Web API
- C#
- PostgreSQL
- Docker

## Getting started

```bash
dotnet restore
dotnet build
dotnet run --project src/JournalService.Api
```

## Running with Docker

```bash
docker build -t job-tracker-app-journal-service .
docker run -p 5003:5003 job-tracker-app-journal-service
```

## Environment variables

| Variable | Description |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string |
| `Auth0__Domain` | Auth0 domain |
| `Auth0__Audience` | Auth0 API audience |
| `Kafka__BootstrapServers` | Kafka broker address |

## Project structure

```
src/
  JournalService.Api/          # Web API entry point, controllers, middleware
  JournalService.Core/         # Domain models, interfaces, business logic
  JournalService.Infrastructure/ # Data access, Kafka, external integrations
tests/
  JournalService.UnitTests/
  JournalService.IntegrationTests/
```
