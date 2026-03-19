using API.Models.Dtos;
using API.Models.PriceModels;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace API.EndPoints
{
    public static class PriceEndPoints
    {
        public static void MapPriceEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/prices").WithTags("Prices");

            /// <summary>
            /// Retrieves a paginated list of all prices.
            /// </summary>
            group.MapGet("/", async (int? pageSize, DateTime? cursor, IPriceService priceService, IMapper mapper, ILogger<IPriceService> logger) =>
            {
                logger.LogInformation("API: Get all prices request received. PageSize: {PageSize}", pageSize);
                try
                {
                    var result = await priceService.GetAllPricesAsync(pageSize ?? 10, cursor);
                    return Results.Ok(new CursorPagedResult<PriceDto>
                    {
                        Data = mapper.Map<List<PriceDto>>(result.Data),
                        NextCursor = result.NextCursor,
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to retrieve prices");
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Gets price details by ID.
            /// </summary>
            group.MapGet("/{id:guid}", async (Guid id, IPriceService priceService, IMapper mapper, ILogger<IPriceService> logger) =>
            {
                logger.LogInformation("API: Get price by ID: {Id}", id);
                try
                {
                    var price = await priceService.GetPriceAsync(id);
                    if (price != null)
                    {
                        return Results.Ok(mapper.Map<PriceDto>(price));
                    }

                    logger.LogWarning("API Warning: Price ID {Id} not found", id);
                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to retrieve price {Id}", id);
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Adds a new base price for a product at a shop.
            /// </summary>
            group.MapPost("/", async (PriceDto priceDto, IPriceService priceService, IMapper mapper, ILogger<IPriceService> logger) =>
            {
                logger.LogInformation("API: Add price request received");
                try
                {
                    var price = mapper.Map<Price>(priceDto);
                    var created = await priceService.AddPriceAsync(price);
                    return Results.Ok(mapper.Map<PriceDto>(created));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to add price");
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Adds a promotion to an existing price.
            /// </summary>
            group.MapPost("/promotions", async (PromotionPriceDto promoDto, IPriceService priceService, IMapper mapper, ILogger<IPriceService> logger) =>
            {
                logger.LogInformation("API: Add promotion request received for Price ID: {PriceId}", promoDto.PriceId);
                try
                {
                    var promo = mapper.Map<PromotionPrice>(promoDto);
                    var created = await priceService.AddPromotionPriceAsync(promo);
                    return Results.Ok(mapper.Map<PromotionPriceDto>(created));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to add promotion");
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Soft-deletes a price record.
            /// </summary>
            group.MapDelete("/{id:guid}", async (Guid id, IPriceService priceService, ILogger<IPriceService> logger) =>
            {
                logger.LogInformation("API: Delete price request ID: {Id}", id);
                try
                {
                    var result = await priceService.DeletePriceAsync(id);
                    if (result)
                    {
                        return Results.NoContent();
                    }

                    return Results.NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "API Error: Failed to delete price {Id}", id);
                    return Results.BadRequest(ex.Message);
                }
            });
        }
    }
}