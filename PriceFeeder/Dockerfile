# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Shared
COPY ./Shared ./Shared

# Copy PriceFeeder
COPY ./PriceFeeder ./PriceFeeder

# Restore
WORKDIR /src/PriceFeeder
RUN dotnet restore "PriceFeeder.csproj"

# Build
RUN dotnet build "PriceFeeder.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PriceFeeder.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PriceFeeder.dll"]
