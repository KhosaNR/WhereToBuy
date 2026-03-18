using API.Models.PriceModels;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IPriceService
{
    Task<Price?> GetPriceAsync(Guid id);
    Task<List<Price>> GetPricesAsync(bool activePromotionsOnly = false);
    Task<List<Price>> GetPricesByShopIdAsync(Guid shopId);
    Task<List<Price>> GetPricesByProductIdAsync(Guid productId);
    Task<Price> AddPriceAsync(Price price);
    Task<bool> UpdatePriceAsync(Price price);
    Task<bool> DeletePriceAsync(Guid priceId);

    Task<PromotionPrice> AddPromotionPriceAsync(PromotionPrice promotionPrice);
    Task<bool> DeletePromotionPriceAsync(Guid promotionPriceId);
}

public class PriceService : IPriceService
{
    private readonly DatabaseContext db;

    public PriceService(DatabaseContext context)
    {
        db = context;
    }

    public async Task<Price?> GetPriceAsync(Guid id)
    {
        return await db.Prices
            .Include(p => p.PromotionPrices)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Price>> GetPricesAsync(bool activePromotionsOnly = false)
    {
        var query = db.Prices.AsQueryable();

        if (activePromotionsOnly)
        {
            var now = DateTime.UtcNow;
            query = query.Include(p => p.PromotionPrices!
                         .Where(pp => pp.StartDate <= now && pp.EndDate >= now));
        }
        else
        {
            query = query.Include(p => p.PromotionPrices);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Price>> GetPricesByShopIdAsync(Guid shopId)
    {
        return await db.Prices
            .Include(p => p.Shop)
            .Include(p => p.PromotionPrices)
            .Where(p => p.ShopId == shopId)
            .ToListAsync();
    }

    public async Task<List<Price>> GetPricesByProductIdAsync(Guid productId)
    {
        return await db.Prices
            .Include(p => p.Product)
            .Include(p => p.PromotionPrices)
            .Where(p => p.ProductId == productId)
            .ToListAsync();
    }

    public async Task<Price> AddPriceAsync(Price price)
    {
        ValidateAmount(price.Amount);

        if (price.ProductId != Guid.Empty && !await db.Products.AnyAsync(p => p.Id == price.ProductId))
        {
            throw new Exception("Product not found.");
        }

        if (price.ShopId != Guid.Empty && !await db.Shops.AnyAsync(s => s.Id == price.ShopId))
        {
            throw new Exception("Shop not found.");
        }

        var existingPrice = await db.Prices.FirstOrDefaultAsync(p =>
            p.ProductId == price.ProductId &&
            p.ShopId == price.ShopId);

        if (existingPrice != null)
        {
            throw new Exception("A base price already exists for this product at this shop.");
        }

        await db.Prices.AddAsync(price);
        await db.SaveChangesAsync();

        return price;
    }

    public async Task<bool> UpdatePriceAsync(Price price)
    {
        ValidateAmount(price.Amount);

        var existingPrice = await db.Prices.FindAsync(price.Id);

        if (existingPrice == null)
        {
            throw new Exception("Price not found.");
        }

        existingPrice.Amount = price.Amount;
        existingPrice.Url = price.Url;
        existingPrice.ProductId = price.ProductId;
        existingPrice.ShopId = price.ShopId;
        existingPrice.PriceDate = price.PriceDate;

        db.Prices.Update(existingPrice);
        await db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePriceAsync(Guid priceId)
    {
        var price = await db.Prices.FindAsync(priceId);

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

    public async Task<PromotionPrice> AddPromotionPriceAsync(PromotionPrice promotionPrice)
    {
        ValidateAmount(promotionPrice.Amount);

        if (promotionPrice.EndDate <= DateTime.UtcNow)
        {
            throw new Exception("Promotion end date must be in the future.");
        }

        if (promotionPrice.StartDate >= promotionPrice.EndDate)
        {
            throw new Exception("Promotion start date must be before the end date.");
        }

        var basePriceExists = await db.Prices.AnyAsync(p => p.Id == promotionPrice.PriceId);
        if (!basePriceExists && promotionPrice.Price == null)
        {
            throw new Exception("Base price not found. Cannot attach promotion.");
        }

        await db.Set<PromotionPrice>().AddAsync(promotionPrice);
        await db.SaveChangesAsync();

        return promotionPrice;
    }

    public async Task<bool> DeletePromotionPriceAsync(Guid promotionPriceId)
    {
        var promo = await db.Set<PromotionPrice>().FindAsync(promotionPriceId);

        if (promo == null)
            throw new Exception("Promotion not found.");

        promo.IsDeleted = true;
        promo.DeletedDate = DateTime.UtcNow;

        db.Set<PromotionPrice>().Update(promo);
        await db.SaveChangesAsync();

        return true;
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Amount should be greater than 0.");
        }
    }
}