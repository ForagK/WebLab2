using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using WebLab2.DataBase;
using WebLab2.Interfaces;
using WebLab2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379")
);

builder.Services.AddScoped<IServiceRepository>(sp =>
{
    var repo = sp.GetRequiredService<ServiceRepository>();
    var redis = sp.GetRequiredService<IConnectionMultiplexer>();

    return new CachedServiceRepository(repo, redis);
});

builder.Services.AddScoped<ILogService>(sp =>
{
    var repo = sp.GetRequiredService<LogService>();
    var redis = sp.GetRequiredService<IConnectionMultiplexer>();

    return new CachedLogService(repo, redis);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("LocalApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7056/");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
