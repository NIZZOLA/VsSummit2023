using Microsoft.EntityFrameworkCore;
using VsSummitApi.Data;
using VsSummitApi;
using VsSummitApi.Helpers;
using System.Reflection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MyConfiguration>(builder.Configuration.GetSection("Configuration"));
builder.ConfigureLogging();
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<VsSummitApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VsSummitApiContext") ?? throw new InvalidOperationException("Connection string 'VsSummitApiContext' not found.")));
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ScanDependencyInjection(Assembly.GetExecutingAssembly(), "Service");

builder.Services.AddLimiterRules();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseOutputCache();
app.UseRateLimiter();

app.MapProductModelEndpoints();
app.MapTestEndpoints();
app.UseHealthChecks("/health");

app.Run();