# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Shared
COPY ./Shared ./Shared

# Copy Portfolio.Api
COPY ./Portfolio.Api ./Portfolio.Api

# Restore dependencies
WORKDIR /src/Portfolio.Api
RUN dotnet restore "Portfolio.Api.csproj"

# Build
RUN dotnet build "Portfolio.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "Portfolio.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
