using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VsSummitApi.Data;
using VsSummitApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VsSummitApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VsSummitApiContext") ?? throw new InvalidOperationException("Connection string 'VsSummitApiContext' not found.")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapProductModelEndpoints();

app.Run();