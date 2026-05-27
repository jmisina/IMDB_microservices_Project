FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/", "src/"]
RUN dotnet restore "src/APIGateways/YARPAPIGateway/YARPAPIGateway.csproj"
RUN dotnet build "src/APIGateways/YARPAPIGateway/YARPAPIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/APIGateways/YARPAPIGateway/YARPAPIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YARPAPIGateway.dll"]
