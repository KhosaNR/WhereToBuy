using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public interface IShopService
    {
        Task<Shop> GetShopAsync(Guid Id);
        List<Shop> SearchShopsByName(string Name);
        Task<Shop> AddShopAsync(Shop shop);
        Task<bool> UpdateShopAsync(Shop shop);
        Task<bool> DeleteShopAsync(Guid ShopId);
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
                logger.LogWarning("AddShopAsync failed: Shop location is missing for shop '{ShopName}'.", shop.Name);
                throw new Exception("Shop location is required.");
            }

            db.Shops.Add(shop);
            await db.SaveChangesAsync();

            logger.LogInformation("Successfully added shop '{ShopName}' with ID {ShopId}.", shop.Name, shop.Id);

            return shop;
        }

        public async Task<bool> UpdateShopAsync(Shop shop)
        {
            var existingShop = await GetShopAsync(shop.Id);

            if (existingShop == null)
            {
                logger.LogWarning("UpdateShopAsync failed: Shop {ShopId} not found.", shop.Id);
                throw new Exception("Shop not found.");
            }

            if (existingShop.Name != shop.Name && await ShopExistsAsync(shop.Name))
            {
                logger.LogWarning("UpdateShopAsync failed: Cannot rename to '{ShopName}', name already exists.", shop.Name);
                throw new Exception("A shop with the same name already exists.");
            }

            db.Shops.Attach(shop);

            map.Map(shop, existingShop);

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully updated shop {ShopId}.", shop.Id);

            return true;
        }

        public async Task<bool> DeleteShopAsync(Guid shopId)
        {
            var shop = await GetShopAsync(shopId);

            if (shop == null)
            {
                logger.LogWarning("DeleteShopAsync failed: Shop {ShopId} not found.", shopId);
                throw new Exception("Shop not found.");
            }

            db.Shops.Attach(shop);

            shop.IsDeleted = true;
            shop.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully soft-deleted shop {ShopId}.", shopId);

            return true;
        }

        private async Task<bool> ShopExistsAsync(string name)
        {
            return await db.Shops.AnyAsync(s => s.Name == name);
        }
    }
}