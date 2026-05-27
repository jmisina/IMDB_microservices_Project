var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Replace placeholders dynamically
var proxyConfig = builder.Configuration.GetSection("ReverseProxy");
var json = System.Text.Json.JsonSerializer.Serialize(proxyConfig.Get<object>());

json = json.Replace("${PRODUCTS_API_URL}", builder.Configuration["PRODUCTS_API_URL"] ?? "")
           .Replace("${USERS_API_URL}", builder.Configuration["USERS_API_URL"] ?? "")
           .Replace("${ORDERS_API_URL}", builder.Configuration["ORDERS_API_URL"] ?? "");

// Load modified config from memory
var memConfig = new ConfigurationBuilder()
    .AddJsonStream(new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
    .Build();

builder.Services.AddReverseProxy().LoadFromConfig(memConfig);

var app = builder.Build();

app.MapReverseProxy();

app.Run();
