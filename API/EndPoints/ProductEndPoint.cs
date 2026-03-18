using API.Services;

namespace API.EndPoints
{
    public static class ProductEndPoint
    {
        public static void MapProductendPoint(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/{id:giud}", async (Guid id, IProductService productService) =>
            {
                var product = await productService.GetProductAsync(id);
            });
        }
    }
}
