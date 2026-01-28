FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY ["./OrderService.Api.OrderService.csproj","OrderService.Api/"]
RUN dotnet restore "./OrderService.Api/OrderService.Api.csproj"
COPY . .
RUN dotnet build "./OrderService.Api/OrderService.Api.csproj" -c Release -o app/build

FROM build as publish
RUN dotnet publish "./OrderService.Api/OrderService.Api.csproj" -c Release -o app/publish /p:UseAppHost=false

FROM base as final
WORKDIR /app
COPY --from=publish app/publish .
ENV MONGO_HOST=localhost
ENV MONGO_PORT=27017
ENV MONGO_DATABASE=OrdersDatabase
ENV UsersMicroserviceName=localhost
ENV UsersMicroservicePort=9090
ENTRYPOINT ["dotnet","OrderService.Api.dll"]