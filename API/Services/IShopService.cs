namespace API.Services
{
    using API.Models;
    using API.Models.Dtos;
    using global::AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public interface IShopService
    {
        Task<CursorPagedResult<Shop>> GetAllShopsAsync(int pageSize, DateTime? cursor);

        Task<Shop?> GetShopAsync(Guid id);

        List<Shop> SearchShopsByName(string name);

        Task<Shop> AddShopAsync(Shop shop);

        Task<bool> UpdateShopAsync(Shop shop);

        Task<bool> DeleteShopAsync(Guid shopId);
    }

    public class ShopService : IShopService
    {
        private readonly DatabaseContext db;
        private readonly IMapper map;
        private readonly ILogger<ShopService> logger;

        public ShopService(DatabaseContext context, IMapper mapper, ILogger<ShopService> logger)
        {
            db = context;
            this.map = mapper;
            this.logger = logger;
        }

        public async Task<CursorPagedResult<Shop>> GetAllShopsAsync(int pageSize, DateTime? cursor)
        {
            logger.LogInformation("Retrieving paged shops. PageSize: {PageSize}", pageSize);
            var query = db.Shops.AsNoTracking().OrderByDescending(s => s.CreatedDate).AsQueryable();

            if (cursor.HasValue)
            {
                query = query.Where(s => s.CreatedDate < cursor.Value);
            }

            var items = await query.Take(pageSize + 1).ToListAsync();
            var hasNextPage = items.Count > pageSize;
            var resultItems = hasNextPage ? items.Take(pageSize).ToList() : items;
            var nextCursor = hasNextPage ? resultItems.Last().CreatedDate : (DateTime?)null;

            return new CursorPagedResult<Shop>
            {
                Data = resultItems,
                NextCursor = nextCursor,
            };
        }

        public async Task<Shop?> GetShopAsync(Guid id)
        {
            logger.LogInformation("Fetching shop by ID: {ShopId}", id);
            return await db.Shops
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public List<Shop> SearchShopsByName(string name)
        {
            logger.LogInformation("Searching shops by name: {ShopName}", name);
            return db.Shops
                .AsNoTracking()
                .Where(s => s.Name.Contains(name))
                .ToList();
        }

        public async Task<Shop> AddShopAsync(Shop shop)
        {
            logger.LogInformation("Adding shop: {ShopName}", shop.Name);
            if (string.IsNullOrEmpty(shop.Name))
            {
                logger.LogWarning("AddShopAsync failed: Shop name is empty.");
                throw new Exception("Shop name cannot be empty.");
            }

            if (await ShopExistsAsync(shop.Name))
            {
                logger.LogWarning("AddShopAsync failed: Shop name '{ShopName}' already exists.", shop.Name);
                throw new Exception("A shop with the same name already exists.");
            }

            if (shop.Location == null || shop.LocationId == Guid.Empty)
            {
                logger.LogWarning("AddShopAsync failed: Shop location is missing.");
                throw new Exception("Shop location is required.");
            }

            db.Shops.Add(shop);
            await db.SaveChangesAsync();

            logger.LogInformation("Shop added successfully with ID: {ShopId}", shop.Id);
            return shop;
        }

        public async Task<bool> UpdateShopAsync(Shop shop)
        {
            logger.LogInformation("Updating shop ID: {ShopId}", shop.Id);
            var existingShop = await GetShopAsync(shop.Id);

            if (existingShop == null)
            {
                logger.LogWarning("Update failed. Shop ID: {ShopId} not found.", shop.Id);
                throw new Exception("Shop not found.");
            }

            if (existingShop.Name != shop.Name && await ShopExistsAsync(shop.Name))
            {
                logger.LogWarning("Update failed. Shop name '{Name}' already exists.", shop.Name);
                throw new Exception("A shop with the same name already exists.");
            }

            db.Shops.Attach(shop);
            map.Map(shop, existingShop);
            await db.SaveChangesAsync();

            logger.LogInformation("Shop ID: {ShopId} updated successfully", shop.Id);
            return true;
        }

        public async Task<bool> DeleteShopAsync(Guid shopId)
        {
            logger.LogInformation("Soft-deleting shop ID: {ShopId}", shopId);
            var shop = await db.Shops.FindAsync(shopId);

            if (shop == null)
            {
                logger.LogWarning("Delete failed. Shop ID: {ShopId} not found.", shopId);
                throw new Exception("Shop not found.");
            }

            shop.IsDeleted = true;
            shop.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();
            logger.LogInformation("Shop ID: {ShopId} soft-deleted successfully", shopId);
            return true;
        }

        private async Task<bool> ShopExistsAsync(string name)
        {
            return await db.Shops.AnyAsync(s => s.Name == name);
        }
    }
}