

namespace Catalog.Endpoints
{
    public static class ProductEndpoints
    {
        public static async Task MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products");

            //Get all
            group.MapGet("/", async (ProductService service) =>
                {
                    var products = await service.GetProductsAsync();
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
            
        }
    }
}
