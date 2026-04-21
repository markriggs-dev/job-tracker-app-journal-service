FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/JournalService.Api/JournalService.Api.csproj", "src/JournalService.Api/"]
COPY ["src/JournalService.Core/JournalService.Core.csproj", "src/JournalService.Core/"]
COPY ["src/JournalService.Infrastructure/JournalService.Infrastructure.csproj", "src/JournalService.Infrastructure/"]
RUN dotnet restore "src/JournalService.Api/JournalService.Api.csproj"
COPY . .
RUN dotnet build "src/JournalService.Api/JournalService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/JournalService.Api/JournalService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JournalService.Api.dll"]
