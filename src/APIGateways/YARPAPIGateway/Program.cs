using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");
builder.Configuration.AddEnvironmentVariables();

var routes = new[] {
    new RouteConfig { RouteId = "catalog-route", ClusterId = "catalog-cluster", Match = new RouteMatch { Path = "/catalog-service/{**catch-all}" }, Transforms = new[] { new Dictionary<string, string> { { "PathRemovePrefix", "/catalog-service" } } } },
    new RouteConfig { RouteId = "user-route", ClusterId = "user-cluster", Match = new RouteMatch { Path = "/user-service/{**catch-all}" }, Transforms = new[] { new Dictionary<string, string> { { "PathRemovePrefix", "/user-service" } } } },
    new RouteConfig { RouteId = "order-route", ClusterId = "order-cluster", Match = new RouteMatch { Path = "/orders-service/{**catch-all}" }, Transforms = new[] { new Dictionary<string, string> { { "PathRemovePrefix", "/orders-service" } } } }
};

var clusters = new[] {
    new ClusterConfig { ClusterId = "catalog-cluster", Destinations = new Dictionary<string, DestinationConfig> { { "cat", new DestinationConfig { Address = builder.Configuration["PRODUCTS_API_URL"] ?? "http://localhost:8080" } } } },
    new ClusterConfig { ClusterId = "user-cluster", Destinations = new Dictionary<string, DestinationConfig> { { "usr", new DestinationConfig { Address = builder.Configuration["USERS_API_URL"] ?? "http://localhost:8080" } } } },
    new ClusterConfig { ClusterId = "order-cluster", Destinations = new Dictionary<string, DestinationConfig> { { "ord", new DestinationConfig { Address = builder.Configuration["ORDERS_API_URL"] ?? "http://localhost:8080" } } } }
};

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);
builder.Services.AddCors(options => options.AddPolicy("AllowFrontend", p => p.WithOrigins("https://imdb-microservices-project.vercel.app", "http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapGet("/", () => "YARP Gateway Running");
app.MapReverseProxy();
app.Run();
