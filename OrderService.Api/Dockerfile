FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    MONGO_HOST=localhost \
    MONGO_PORT=27017 \
    MONGO_DATABASE=OrdersDatabase \
    UsersMicroserviceName=localhost \
    UsersMicroservicePort=8080 \
    ProductsMicroserviceName=localhost \
    ProductsMicroservicePort=8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OrderService.Api/OrderService.Api.csproj", "OrderService.Api/"]
RUN dotnet restore "OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c $configuration -o /app/build

FROM build AS publish 
ARG configuration=Release
RUN dotnet publish "OrderService.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]
