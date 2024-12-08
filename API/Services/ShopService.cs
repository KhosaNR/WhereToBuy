using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        public ShopService(DatabaseContext context, IMapper mapper)
        {
            db = context;
            this.map = mapper;
        }

        public async Task<Shop?> GetShopAsync(Guid id)
        {
            return await db.Shops.FindAsync(id);
        }

        public List<Shop> SearchShopsByName(string name)
        {
            return db.Shops
                .Where(s => s.Name.Contains(name))
                .ToList();
        }

        public async Task<Shop> AddShopAsync(Shop shop)
        {
            if (await ShopExistsAsync(shop.Name))
            {
                throw new Exception("A shop with the same name already exists.");
            }

            if (string.IsNullOrEmpty(shop.Name))
            {
                throw new Exception("Shop name cannot be empty.");
            }

            if (shop.Location == null || shop.LocationId == Guid.Empty)
            {
                throw new Exception("Shop location is required.");
            }

            db.Shops.Add(shop);
            await db.SaveChangesAsync();

            return shop;
        }

        public async Task<bool> UpdateShopAsync(Shop shop)
        {
            var existingShop = await GetShopAsync(shop.Id);

            if (existingShop == null)
            {
                throw new Exception("Shop not found.");
            }

            if (existingShop.Name != shop.Name && await ShopExistsAsync(shop.Name))
            {
                throw new Exception("A shop with the same name already exists.");
            }

            map.Map(shop, existingShop);

            db.Shops.Update(existingShop);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteShopAsync(Guid shopId)
        {
            var shop = await GetShopAsync(shopId);

            if (shop == null)
            {
                throw new Exception("Shop not found.");
            }

            shop.IsDeleted = true;
            shop.DeletedDate = DateTime.UtcNow;
            //shop.DeletedById = Guid.Parse("YourDeletedById"); // Replace with actual deleted by id

            db.Shops.Update(shop);
            await db.SaveChangesAsync();

            return true;
        }

        private async Task<bool> ShopExistsAsync(string name)
        {
            return await db.Shops.AnyAsync(s => s.Name == name);
        }
    }
}
