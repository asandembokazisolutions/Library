# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY Library/Library.csproj Library/
RUN dotnet restore Library/Library.csproj

# Copy everything else and publish
COPY . .
RUN dotnet publish Library/Library.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

# Render injects a PORT env var at runtime; bind Kestrel to it here
# (ENV is fixed at build time, so this must happen in the entrypoint, not via ENV)
ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet Library.dll"]
