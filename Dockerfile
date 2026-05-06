FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/JournalService.Api/JournalService.Api.csproj", "src/JournalService.Api/"]
COPY ["src/JournalService.Core/JournalService.Core.csproj", "src/JournalService.Core/"]
COPY ["src/JournalService.Infrastructure/JournalService.Infrastructure.csproj", "src/JournalService.Infrastructure/"]
RUN dotnet restore "src/JournalService.Api/JournalService.Api.csproj"

COPY . .
RUN dotnet publish "src/JournalService.Api/JournalService.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JournalService.Api.dll"]
