﻿

namespace Catalog.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products");

            //Get all
            group.MapGet("/", async (ProductService service) =>
                {
                    var products = await service.GetProductsAsync();
                    return Results.Ok(products);
                })
                .WithName("GetAllProducts")
                .Produces<List<Product>>(StatusCodes.Status200OK);

            //Get by Id

            group.MapGet("/{id}", async (int id, ProductService service) =>
            {
                var product = await service.GetProductByIdAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            })
                .WithName("GetProductById")
                .Produces<Product>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Post (Create)
            group.MapPost("/", async (Product product, ProductService service) =>
            {
                await service.CreateProductAsync(product);
                return Results.Created($"/products/{product.Id}", product);

            })
                .WithName("CreateProduct")
                .Produces<Product>(StatusCodes.Status201Created);

            //Put (Update)

            group.MapPut("/{id}", async (int id, Product inputProduct, ProductService service) =>
            {
                var updatedProduct = await service.GetProductByIdAsync(id);
                if (updatedProduct is null)
                {
                    return Results.NotFound();
                }

                await service.UpdateProductAsync(updatedProduct, inputProduct);
                return Results.NoContent();
            })
                .WithName("UpdateProduct")
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status204NoContent);


            // Delete

            group.MapDelete("/{id}", async (int id, ProductService service) =>
            {
                var deletedProduct = await service.GetProductByIdAsync(id);
                if (deletedProduct is null)
                {
                    return Results.NoContent();
                }

                await service.DeleteProductAsync(deletedProduct);
                return Results.NoContent();
            })
                .WithName("DeleteProduct")
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status204NoContent);

        }
    }
}
