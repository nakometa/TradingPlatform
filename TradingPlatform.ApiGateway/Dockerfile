# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy ApiGateway project
COPY ./TradingPlatform.ApiGateway ./TradingPlatform.ApiGateway

# Restore dependencies
WORKDIR /src/TradingPlatform.ApiGateway
RUN dotnet restore "TradingPlatform.ApiGateway.csproj"

# Build
RUN dotnet build "TradingPlatform.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "TradingPlatform.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingPlatform.ApiGateway.dll"]
