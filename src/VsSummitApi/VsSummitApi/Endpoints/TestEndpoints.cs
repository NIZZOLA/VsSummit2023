using Microsoft.Extensions.Options;
using VsSummitApi.Interfaces.Services;

namespace VsSummitApi.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Test").WithTags("Test");

        group.MapGet("/", async (IOptions<MyConfiguration> options) =>
        {
            return options.Value.Secret;
        })
        .WithName("GetSecret");

        group.MapGet("/getall", async (IProductService service) =>
        {
            return service.GetAll();
        })
        .WithName("GetAll");

        group.MapGet("/cachedefault", () =>
        {
            var response = "Resposta gerada em:" + DateTime.Now.ToString();
            return response;
        })
        .CacheOutput()
        .WithName("CacheDefault")
        .WithOpenApi();

        group.MapGet("/cachelong", (string param) =>
        {
            var response = "Resposta gerada em:" + DateTime.Now.ToString();
            return response;
        })
        .CacheOutput(policy =>
        {
            policy.SetVaryByQuery("param");
            policy.Expire(TimeSpan.FromSeconds(60));
        })
        .WithName("CacheLong")
        .WithOpenApi();

        group.MapGet("/ratelimited", () =>
        {
            var response = "Resposta gerada em:" + DateTime.Now.ToString();
            return response;
        })
        .WithName("RateLimited")
        .RequireRateLimiting("fixed")
        .WithOpenApi();

        group.MapGet("/rateperiodwith3queue", () =>
        {
            var response = "Resposta gerada em:" + DateTime.Now.ToString();
            return response;
        })
        .WithName("RateLimitedTo3queue")
        .RequireRateLimiting("period3")
        .WithOpenApi();
    }
}
