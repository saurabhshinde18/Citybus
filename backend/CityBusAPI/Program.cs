using CityBusAPI.Data;
using CityBusAPI.DTOs;
using CityBusAPI.Models;
using CityBusAPI.Repositories;
using CityBusAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Route = CityBusAPI.Models.Route;
using MySqlConnector;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Try to use MySQL, fall back to local SQLite file when MySQL is unavailable (development convenience)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
bool usingSqliteFallback = false;
try
{
    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        using var testConn = new MySqlConnection(connectionString);
        testConn.Open();
        testConn.Close();
    }
    else
    {
        usingSqliteFallback = true;
    }
}
catch
{
    usingSqliteFallback = true;
}

if (!usingSqliteFallback)
{
    builder.Services.AddDbContext<CityBusDbContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });
}
else
{
    // Use file-based SQLite in the working directory for local development
    var sqliteFile = Path.Combine(Directory.GetCurrentDirectory(), "citybus_local.db");
    var sqliteConn = $"Data Source={sqliteFile}";
    builder.Services.AddDbContext<CityBusDbContext>(options =>
    {
        options.UseSqlite(sqliteConn);
    });
}

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAudioService, AudioService>();

// Register repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAudioRepository, AudioRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();

// Add JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add CORS
var corsSettings = builder.Configuration.GetSection("Cors");
var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policyBuilder =>
    {
        policyBuilder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CityBusDbContext>();
    if (!usingSqliteFallback)
    {
        dbContext.Database.Migrate();
    }
    else
    {
        // For SQLite fallback, ensure database and tables are created
        dbContext.Database.EnsureCreated();
    }

    // Seed data
    SeedDatabase(dbContext);
}

app.Run();

void SeedDatabase(CityBusDbContext dbContext)
{
    // Check if data already exists
    if (dbContext.Users.Any())
        return;

    var passwordService = app.Services.GetRequiredService<IPasswordService>();

    // Seed users
    var adminUser = new User
    {
        Name = "Admin User",
        Email = "admin@citybus.gov",
        PasswordHash = passwordService.HashPassword("Password@123"),
        Role = "Admin",
        IsActive = true
    };

    dbContext.Users.Add(adminUser);
    dbContext.SaveChanges();

    // Seed routes
    var routes = new[]
    {
        new Route { Name = "Route 1", StartPoint = "Central Station", EndPoint = "Airport", DistanceKm = 25 },
        new Route { Name = "Route 2", StartPoint = "Downtown", EndPoint = "University", DistanceKm = 15 },
        new Route { Name = "Route 5", StartPoint = "Railway Station", EndPoint = "Bus Terminal", DistanceKm = 12 },
    };

    dbContext.Routes.AddRange(routes);
    dbContext.SaveChanges();

    // Seed buses
    var buses = new[]
    {
        new Bus { BusNumber = "B001", RouteId = 1, Capacity = 45, Status = "Active" },
        new Bus { BusNumber = "B002", RouteId = 2, Capacity = 45, Status = "Active" },
        new Bus { BusNumber = "B003", RouteId = 3, Capacity = 50, Status = "Inactive" },
    };

    dbContext.Buses.AddRange(buses);
    dbContext.SaveChanges();

    // Seed audio files
    var audioFiles = new[]
    {
        new AudioFile { Name = "Route 5 Announcement", Category = "Route", DurationSeconds = 15, IsActive = true, Description = "Standard route announcement" },
        new AudioFile { Name = "Emergency Alert Siren", Category = "Emergency", DurationSeconds = 20, IsActive = true, Description = "High priority emergency alert" },
        new AudioFile { Name = "Festival Message - Diwali", Category = "Festival", DurationSeconds = 45, IsActive = true, Description = "Festival celebration message" },
    };

    dbContext.AudioFiles.AddRange(audioFiles);
    dbContext.SaveChanges();
}
