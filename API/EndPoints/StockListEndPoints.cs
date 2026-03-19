namespace API.EndPoints
{
    using API.Models.Dtos;
    using API.Services;
    using global::AutoMapper;

    public static class StockListEndPoints
    {
        public static void MapStockListEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stocklists").WithTags("StockLists");

            /// <summary>
            /// Retrieves paginated stock lists owned by or shared with the user.
            /// </summary>
            /// <param name="userId">The ID of the current user.</param>
            group.MapGet("/", async (Guid userId, int? pageSize, DateTime? cursor, IStockListService service, IMapper mapper) =>
            {
                var result = await service.GetAllStockListsAsync(pageSize ?? 10, cursor, userId);
                return Results.Ok(new CursorPagedResult<StockListDto>
                {
                    Data = mapper.Map<List<StockListDto>>(result.Data),
                    NextCursor = result.NextCursor,
                });
            });

            /// <summary>
            /// Creates a new empty stock list.
            /// </summary>
            group.MapPost("/", async (Guid userId, string name, IStockListService service) =>
            {
                try
                {
                    await service.CreateStockList(userId, name);
                    return Results.Created();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Shares a stock list with another user.
            /// </summary>
            group.MapPost("/{id:guid}/share", async (Guid id, Guid targetUserId, Guid currentUserId, IStockListService service) =>
            {
                try
                {
                    await service.AddUser(id, targetUserId, currentUserId);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            /// <summary>
            /// Soft-deletes a stock list.
            /// </summary>
            group.MapDelete("/{id:guid}", async (Guid id, Guid currentUserId, IStockListService service) =>
            {
                var result = await service.DeleteStockList(id, currentUserId);
                return result ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}