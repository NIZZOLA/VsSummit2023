using VsSummitApi;
using VsSummitApi.Helpers;
using VsSummitApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MyConfiguration>(builder.Configuration.GetSection("Configuration"));
builder.ConfigureLogging();
builder.Services.AddStartupConfig(builder.Configuration);

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

app.AddEndpoints();
app.UseHealthChecks("/health");

app.Run();