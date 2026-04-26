# Base image for Azure Functions .NET 8 Isolated
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

# Build image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["PACHA_FIT.csproj", "./"]
RUN dotnet restore "PACHA_FIT.csproj"

# Copy everything else
COPY . .

# Build and publish
# We use -v q to keep output clean, but ensure it's a Release build
RUN dotnet publish "PACHA_FIT.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .

# Environment variables for Azure Functions
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    FUNCTIONS_WORKER_RUNTIME=dotnet-isolated
