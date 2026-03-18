using API.Models;
using API.Models.Dtos;
using API.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace API.EndPoints
{
    public static class ShopEndPoints
    {
        public static void MapShopEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/shops").WithTags("Shops");

            group.MapGet("/{id:guid}", async (Guid id, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("Fetching shop by ID: {ShopId}", id);
                var shop = await shopService.GetShopAsync(id);

                if (shop is not null)
                {
                    return Results.Ok(mapper.Map<ShopDto>(shop));
                }

                logger.LogWarning("Shop with ID: {ShopId} was not found", id);
                return Results.NotFound();
            });

            group.MapGet("/search", async (string searchString, IShopService shopService, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("Searching shops with query: {SearchString}", searchString);
                var shops = shopService.SearchShopsByName(searchString);
                return Results.Ok(shops);
            });

            group.MapPost("/", async (ShopDto shopDto, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("Attempting to create a new shop");
                try
                {
                    var shop = mapper.Map<Shop>(shopDto);
                    var createdShop = await shopService.AddShopAsync(shop);

                    logger.LogInformation("Successfully created shop");
                    return Results.Ok(mapper.Map<ShopDto>(createdShop));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while creating a shop");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapPut("/", async (ShopDto shopDto, IShopService shopService, IMapper mapper, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("Attempting to update shop: {@ShopDto}", shopDto);
                try
                {
                    var shop = mapper.Map<Shop>(shopDto);
                    var updated = await shopService.UpdateShopAsync(shop);

                    if (updated)
                    {
                        logger.LogInformation("Successfully updated shop");
                        return Results.NoContent();
                    }

                    logger.LogWarning("Failed to update shop as it was not found: {@ShopDto}", shopDto);
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while updating the shop");
                    return Results.BadRequest(ex.Message);
                }
            });

            group.MapDelete("/{id:guid}", async (Guid id, IShopService shopService, ILogger<IShopService> logger) =>
            {
                logger.LogInformation("Attempting to delete shop by ID: {ShopId}", id);
                var result = await shopService.DeleteShopAsync(id);

                if (result)
                {
                    logger.LogInformation("Successfully deleted shop by ID: {ShopId}", id);
                    return Results.NoContent();
                }

                logger.LogWarning("Failed to delete shop with ID: {ShopId} because it was not found", id);
                return Results.NotFound();
            });
        }
    }
}