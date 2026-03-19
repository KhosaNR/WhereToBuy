namespace API.EndPoints
{
    using API.Models;
    using API.Models.Dtos;
    using API.Services;
    using global::AutoMapper;

    public static class StockProductEndPoints
    {
        public static void MapStockProductEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stocklists/{stockListId:guid}/products").WithTags("StockListProducts");

            /// <summary>
            /// Adds or updates a product's quantity within a specific stock list.
            /// </summary>
            /// <param name="stockListId">The ID of the target list.</param>
            /// <param name="productDto">Product details and quantity.</param>
            /// <param name="currentUserId">ID of the user performing the action.</param>
            group.MapPost("/", async (Guid stockListId, StockListProductDto productDto, Guid currentUserId, IStockProductService service, IMapper mapper) =>
            {
                try
                {
                    var product = mapper.Map<StockListProduct>(productDto);
                    await service.AddOrUpdateProduct(stockListId, product, currentUserId);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Removes a product from a stock list.
            /// </summary>
            group.MapDelete("/{productId:guid}", async (Guid stockListId, Guid productId, Guid currentUserId, IStockProductService service) =>
            {
                try
                {
                    var result = await service.RemoveProduct(stockListId, productId, currentUserId);
                    return result ? Results.NoContent() : Results.NotFound();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}