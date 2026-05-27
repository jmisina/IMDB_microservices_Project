FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/", "src/"]
RUN dotnet restore "src/Services/OrdersAPI/OrdersAPI.csproj"
RUN dotnet build "src/Services/OrdersAPI/OrdersAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Services/OrdersAPI/OrdersAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersAPI.dll"]
