using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using OrdersAPI.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Port binding for cloud hosting
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://+:{port}");

// Load .env file
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
if (File.Exists(dotenv))
{
    foreach (var line in File.ReadAllLines(dotenv))
    {
        var parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) continue;
        Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
    }
}

// Re-sync configuration
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("Database") ?? "";
connectionString = connectionString
    .Replace("${DB_HOST}", builder.Configuration["DB_HOST"])
    .Replace("${DB_PORT}", builder.Configuration["DB_PORT"] ?? "5432")
    .Replace("${DB_NAME}", builder.Configuration["DB_NAME"])
    .Replace("${DB_USER}", builder.Configuration["DB_USER"])
    .Replace("${DB_PASS}", builder.Configuration["DB_PASS"]);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddControllers();
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddHttpClient("ProductsAPI", client =>
{
    var baseAddress = builder.Configuration["ProductsApiSettings:BaseAddress"] ?? "http://productsapi:8080";
    client.BaseAddress = new Uri(baseAddress.Replace("${PRODUCTS_API_BASE_ADDRESS}", builder.Configuration["PRODUCTS_API_BASE_ADDRESS"] ?? "http://productsapi:8080"));
});


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrManager", policy => 
        policy.RequireRole("ADMIN", "MANAGER"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var secret = builder.Configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT Secret is not configured.");
        }

        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.Migrate();
}

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API");
        options.RoutePrefix = "orders/swagger"; 
    });
}


app.UseAuthentication();
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
