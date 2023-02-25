using VsSummitApi;
using VsSummitApi.Helpers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MyConfiguration>(builder.Configuration.GetSection("Configuration"));
builder.ConfigureLogging();
builder.Services.AddHealthChecks();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureOutputCache();
builder.Services.AddLimiterRules();
builder.Services.ScanDependencyInjection(Assembly.GetExecutingAssembly(), "Service");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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