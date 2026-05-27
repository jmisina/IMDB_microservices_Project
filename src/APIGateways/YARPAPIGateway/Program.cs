using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Model;

var builder = WebApplication.CreateBuilder(args);

// Port binding for cloud hosting
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://imdb-microservices-project.vercel.app", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.Use((context, next) =>
{
    var proxyFeature = context.GetReverseProxyFeature();
    if (proxyFeature != null)
    {
        var destination = proxyFeature.AvailableDestinations.FirstOrDefault();
        Console.WriteLine($"Proxying request to: {destination?.Model.Config.Address}");
    }
    return next();
});

app.UseCors("AllowFrontend");
app.MapReverseProxy();

app.Run();
