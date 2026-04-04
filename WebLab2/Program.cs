using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StackExchange.Redis;
using Microsoft.OpenApi;
using WebLab2.DataBase;
using WebLab2.Hubs;
using WebLab2.Interfaces;
using WebLab2.Services;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddSingleton<IStreamService, StreamService>();

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

builder.Services.AddHostedService<UptimeMonitorWorker>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("LocalApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7056/");
});

builder.Services.AddSignalR();
builder.Services.AddRazorPages();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
}).AddMvc();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "v1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "v2", Version = "v2" });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger("/openapi/{documentName}.json");
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("UpTulse API Docs");
        options.WithTheme(ScalarTheme.Mars);
    });
}

app.MapHub<MonitoringHub>("/hubs/monitoring");

app.Run();
