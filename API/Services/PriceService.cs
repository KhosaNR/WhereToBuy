using API.Models.PriceModels;
using API.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

public interface IPriceService
{
    Task<T?> GetPriceAsync<T>(Guid id) where T: Price;
    Task<List<T>?> GetPricesAsync<T>(bool? isActive = null) where T: Price;
    Task<List<T>?> GetPricesByShopIdAsync<T>(Guid shopId) where T: Price;
    Task<List<T>?> GetPricesByProductIdAsync<T>(Guid productId) where T: Price;
    Task<T> AddPriceAsync<T>(T price) where T : Price;
    Task<bool> UpdatePriceAsync<T>(T price) where T: Price;
    Task<bool> DeletePriceAsync<T>(Guid priceId) where T: Price;
}

public class PriceService : IPriceService
{
    private readonly DatabaseContext db;

    public PriceService(DatabaseContext context)
    {
        db = context;
    }

    public async Task<T?> GetPriceAsync<T>(Guid id) where T: Price
    {
        return await db.Prices.FindAsync(id) as T;
    }

    public async Task<List<T>?> GetPricesAsync<T>(bool? isActive = null) where T : Price
    {
        var query = db.Prices.OfType<T>().AsQueryable();

        if (isActive.HasValue)
        {
            if (isActive.Value && typeof(T) == typeof(PromotionPrice))
            {
                query = query.Where(pp => (pp as PromotionPrice).EndDate >= DateTime.UtcNow);
            }
            else if (!isActive.Value && typeof(T) == typeof(PromotionPrice))
            {
                query = query.Where(pp => (pp as PromotionPrice).EndDate < DateTime.UtcNow);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<List<T>?> GetPricesByShopIdAsync<T>(Guid shopId) where T: Price
    {
        return await db.Prices
                        .Include(p => p.Shop)
                        .Where(p => p.ShopId == shopId)
                        .ToListAsync() as List<T>;
    }

    public async Task<List<T>?> GetPricesByProductIdAsync<T>(Guid productId) where T: Price
    {
        return await db.Prices
                        .Include(p => p.Product)
                        .Where(p => p.ProductId == productId)
                        .ToListAsync() as List<T>;
    }

    public async Task<T> AddPriceAsync<T>(T price) where T : Price
    {
        ValidatePrice(price);

        if (price.ProductId != Guid.Empty)
        {
            var product = await db.Products.FindAsync(price.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
        }

        if (price.ShopId != Guid.Empty)
        {
            var shop = await db.Shops.FindAsync(price.ShopId);
            if (shop == null)
            {
                throw new Exception("Shop not found.");
            }
        }

        Price? existingPrice;

        if (price is PromotionPrice promotionPrice)
        {
            if (promotionPrice.EndDate <= DateTime.UtcNow)
            {
                throw new Exception("Invalid promotion price end date. Promotion price cannot be less than now.");
            }

            existingPrice = await db.Prices.OfType<PromotionPrice>().FirstOrDefaultAsync(p =>
                p.ProductId == promotionPrice.ProductId &&
                p.ShopId == promotionPrice.ShopId &&
                p.IsPack == promotionPrice.IsPack &&
                p.IsPromotion == true &&
                p.IsBulk == promotionPrice.IsBulk);

        }
        else
        {
            existingPrice = await db.Prices.FirstOrDefaultAsync(p =>
                p.ProductId == price.ProductId &&
                p.ShopId == price.ShopId &&
                p.IsPack == price.IsPack);
        }

        if (existingPrice != null)
        {
            throw new Exception("Price already exists for the product and shop.");
        }

        if (existingPrice != null)
        {
            throw new Exception("Price already exists for the product and shop.");
        }

        await db.Prices.AddAsync(price);
        await db.SaveChangesAsync();

        return price;
    }

    public async Task<bool> UpdatePriceAsync<T>(T price) where T: Price
    {
        ValidatePrice(price);

        var existingPrice = await GetPriceAsync<T>(price.Id) ;

        if (existingPrice == null)
        {
            throw new Exception("Price not found.");
        }

        if (price.ProductId != Guid.Empty)
        {
            var product = await db.Products.FindAsync(price.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
        }

        if (price.ShopId != Guid.Empty)
        {
            var shop = await db.Shops.FindAsync(price.ShopId);
            if (shop == null)
            {
                throw new Exception("Shop not found.");
            }
        }

        existingPrice.Amount = price.Amount;
        existingPrice.Url = price.Url;
        existingPrice.ProductId = price.ProductId;
        existingPrice.ShopId = price.ShopId;
        existingPrice.PriceDate = price.PriceDate;
        existingPrice.IsPack = price.IsPack;
        existingPrice.UnitsPerPack = price.UnitsPerPack;

        db.Prices.Update(existingPrice);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePriceAsync<T>(Guid priceId) where T: Price
    {
        var price = await GetPriceAsync<T>(priceId);

        if (price == null)
        {
            throw new Exception("Price not found.");
        }

        price.IsDeleted = true;
        price.DeletedDate = DateTime.UtcNow;

        db.Prices.Update(price);
        await db.SaveChangesAsync();

        return true;
    }

    private void ValidatePrice<T>(T price) where T: Price
    {
        if (price.Amount <= 0)
        {
            throw new Exception("Amount should be greater than 0.");
        }

        if (price.IsPack && price.UnitsPerPack <= 1)
        {
            throw new Exception("Units per pack should be more than 1 is price per pack.");
        }

        if (!price.IsPack && (price.UnitsPerPack >= 1 || price.UnitsPerPack<= 0))
        {
            throw new Exception("Cannot set units per pack if price is not for pack.");
        }
    }
}