using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using VsSummitApi.Data;
using VsSummitApi.Models;

namespace VsSummitApi.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductModelEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product").WithTags("Product").RequireAuthorization("Administrator");

        group.MapGet("/", async (VsSummitApiContext db) =>
        {
            return await db.ProductModel.ToListAsync();
        })
        .WithName("GetAllProductModels").Produces<ICollection<ProductModel>>();

        group.MapGet("/{id}", async Task<Results<Ok<ProductModel>, NotFound>> (int id, VsSummitApiContext db) =>
        {
            return await db.ProductModel.FindAsync(id)
                is ProductModel model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetProductModelById").Produces<ProductModel>().Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, ProductModel productModel, VsSummitApiContext db) =>
        {
            var foundModel = await db.ProductModel.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            db.Update(productModel);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateProductModel").Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (ProductModel productModel, VsSummitApiContext db) =>
        {
            db.ProductModel.Add(productModel);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/ProductModel/{productModel.Id}", productModel);
        })
        .WithName("CreateProductModel").Produces<ProductModel>(StatusCodes.Status201Created);

        group.MapDelete("/{id}", async Task<Results<Ok<ProductModel>, NotFound>> (int id, VsSummitApiContext db) =>
        {
            if (await db.ProductModel.FindAsync(id) is ProductModel productModel)
            {
                db.ProductModel.Remove(productModel);
                await db.SaveChangesAsync();
                return TypedResults.Ok(productModel);
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteProductModel").Produces(StatusCodes.Status404NotFound).Produces<ProductModel>(StatusCodes.Status200OK);
    }
}
