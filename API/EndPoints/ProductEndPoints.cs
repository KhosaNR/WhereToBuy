namespace API.EndPoints
{
    using API.Models;
    using API.Models.Dtos;
    using API.Services;
    using global::AutoMapper;
    using Microsoft.Extensions.Logging;

    public static class ProductEndPoints
    {
        public static void MapProductEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/", async (int? pageSize, DateTime? cursor, IProductService productService, IMapper mapper, ILogger<IProductService> logger) =>
            {
                var limit = pageSize ?? 10;
                logger.LogInformation("Fetching products with pageSize: {PageSize}, cursor: {Cursor}", limit, cursor);

                var result = await productService.GetAllProductsAsync(limit, cursor);

                var response = new CursorPagedResult<ProductDto>
                {
                    Data = mapper.Map<List<ProductDto>>(result.Data),
                    NextCursor = result.NextCursor,
                };

                return Results.Ok(response);
            });

            group.MapGet("/{id:guid}", async (Guid id, IProductService productService, IMapper mapper, ILogger<IProductService> logger) =>
            {
                logger.LogInformation("Fetching product by ID: {ProductId}", id);
                var product = await productService.GetProductAsync(id);

                if (product is not null)
                {
                    return Results.Ok(mapper.Map<ProductDto>(product));
                }

                logger.LogWarning("Product with ID: {ProductId} was not found", id);
                return Results.NotFound();
            });

            group.MapGet("/search", async (string searchString, IProductService productService, ILogger<IProductService> logger) =>
            {
                logger.LogInformation("Searching products with query: {SearchString}", searchString);
                var products = await productService.SearchProductAsync(searchString);
                return Results.Ok(products);
            });

            group.MapPost("/", async (ProductDto productDto, IProductService productService, IMapper mapper, ILogger<IProductService> logger) =>
            {
                logger.LogInformation("Attempting to create a new product");
                try
                {
                    var product = mapper.Map<Product>(productDto);
                    await productService.AddProductAsync(product);

                    logger.LogInformation("Successfully created product");
                    return Results.Ok(mapper.Map<ProductDto>(product));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while creating a product");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapPut("/", async (ProductDto productDto, IProductService productService, IMapper mapper, ILogger<IProductService> logger) =>
            {
                logger.LogInformation("Attempting to update product: {@ProductDto}", productDto);
                try
                {
                    var product = mapper.Map<Product>(productDto);
                    var updated = await productService.UpdateProductAsync(product);

                    if (updated)
                    {
                        logger.LogInformation("Successfully updated product");
                        return Results.NoContent();
                    }

                    logger.LogWarning("Failed to update product as it was not found: {@ProductDto}", productDto);
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while updating the product");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapDelete("/{id:guid}", async (Guid id, IProductService productService, ILogger<IProductService> logger) =>
            {
                logger.LogInformation("Attempting to delete product by ID: {ProductId}", id);
                var result = await productService.DeleteProductAsync(id);

                if (result)
                {
                    logger.LogInformation("Successfully deleted product by ID: {ProductId}", id);
                    return Results.NoContent();
                }

                logger.LogWarning("Failed to delete product with ID: {ProductId} because it was not found", id);
                return Results.NotFound();
            });
        }
    }
}