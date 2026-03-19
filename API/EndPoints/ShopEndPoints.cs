namespace API.EndPoints
{
    using API.Models;
    using API.Models.Dtos;
    using API.Services;
    using global::AutoMapper;
    using Microsoft.Extensions.Logging;

    public static class ShopEndPoints
    {
        public static void MapShopEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/shops").WithTags("Shops");

            group.MapGet("/", async (int? pageSize, DateTime? cursor, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("API: Get shops request");
                try
                {
                    var result = await shopService.GetAllShopsAsync(pageSize ?? 10, cursor);
                    return Results.Ok(new CursorPagedResult<ShopDto>
                    {
                        Data = mapper.Map<List<ShopDto>>(result.Data),
                        NextCursor = result.NextCursor,
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to retrieve shops");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapGet("/{id:guid}", async (Guid id, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("API: Get shop ID: {Id}", id);
                try
                {
                    var shop = await shopService.GetShopAsync(id);
                    if (shop != null)
                    {
                        return Results.Ok(mapper.Map<ShopDto>(shop));
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to retrieve shop {Id}", id);
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapPost("/", async (ShopDto shopDto, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("API: Create shop request");
                try
                {
                    var shop = mapper.Map<Shop>(shopDto);
                    var createdShop = await shopService.AddShopAsync(shop);
                    return Results.Ok(mapper.Map<ShopDto>(createdShop));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to create shop");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapPut("/", async (ShopDto shopDto, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("API: Update shop request for ID: {Id}", shopDto.Id);
                try
                {
                    var shop = mapper.Map<Shop>(shopDto);
                    var updated = await shopService.UpdateShopAsync(shop);
                    if (updated)
                    {
                        return Results.NoContent();
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to update shop");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapDelete("/{id:guid}", async (Guid id, IShopService shopService, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("API: Delete shop request for ID: {Id}", id);
                try
                {
                    var result = await shopService.DeleteShopAsync(id);
                    if (result)
                    {
                        return Results.NoContent();
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to delete shop {Id}", id);
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}