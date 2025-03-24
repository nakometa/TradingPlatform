# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy Shared
COPY ./Shared ./Shared

# Copy Order.Api
COPY ./Order.Api ./Order.Api

# Restore
WORKDIR /src/Order.Api
RUN dotnet restore "Order.Api.csproj"

# Build
RUN dotnet build "Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Api.dll"]
