using Microsoft.Extensions.Options;
using VsSummitApi.Data;
using VsSummitApi.Interfaces.Services;

namespace VsSummitApi;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Test");

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
    }
}
