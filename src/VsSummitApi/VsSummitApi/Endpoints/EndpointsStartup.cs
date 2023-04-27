namespace VsSummitApi.Endpoints;

public static class EndpointsStartup
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapProductModelEndpoints();
        app.MapTestEndpoints();
    }
}
