using API.Models.PriceModels;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<PriceService> logger;

    public PriceService(DatabaseContext context, ILogger<PriceService> logger)
    {
        db = context;
        this.logger = logger;
    }

    public async Task<Price?> GetPriceAsync(Guid id)
    {
        logger.LogInformation("Fetching price for ID: {PriceId}", id);

        return await db.Prices
            .AsNoTracking()
            .Include(p => p.PromotionPrices)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Price>> GetPricesAsync(bool activePromotionsOnly = false)
    {
        logger.LogInformation("Fetching all prices. ActivePromotionsOnly: {ActivePromotionsOnly}", activePromotionsOnly);

        var query = db.Prices.AsNoTracking().AsQueryable();

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
        logger.LogInformation("Fetching prices for Shop ID: {ShopId}", shopId);

        return await db.Prices.AsNoTracking()
            .Include(p => p.Shop)
            .Include(p => p.PromotionPrices)
            .Where(p => p.ShopId == shopId)
            .ToListAsync();
    }

    public async Task<List<Price>> GetPricesByProductIdAsync(Guid productId)
    {
        logger.LogInformation("Fetching prices for Product ID: {ProductId}", productId);

        return await db.Prices.AsNoTracking()
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
            logger.LogWarning("AddPriceAsync failed: Product {ProductId} not found.", price.ProductId);
            throw new Exception("Product not found.");
        }

        if (price.ShopId != Guid.Empty && !await db.Shops.AnyAsync(s => s.Id == price.ShopId))
        {
            logger.LogWarning("AddPriceAsync failed: Shop {ShopId} not found.", price.ShopId);
            throw new Exception("Shop not found.");
        }

        var existingPrice = await db.Prices.FirstOrDefaultAsync(p =>
            p.ProductId == price.ProductId &&
            p.ShopId == price.ShopId);

        if (existingPrice != null)
        {
            logger.LogWarning("AddPriceAsync failed: A base price already exists for Product {ProductId} at Shop {ShopId}.", price.ProductId, price.ShopId);
            throw new Exception("A base price already exists for this product at this shop.");
        }

        await db.Prices.AddAsync(price);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully added new price {PriceId} for Product {ProductId} at Shop {ShopId}.", price.Id, price.ProductId, price.ShopId);

        return price;
    }

    public async Task<bool> UpdatePriceAsync(Price price)
    {
        ValidateAmount(price.Amount);

        var existingPrice = await db.Prices.FindAsync(price.Id);

        if (existingPrice == null)
        {
            logger.LogWarning("UpdatePriceAsync failed: Price {PriceId} not found.", price.Id);
            throw new Exception("Price not found.");
        }

        existingPrice.Amount = price.Amount;
        existingPrice.Url = price.Url;
        existingPrice.ProductId = price.ProductId;
        existingPrice.ShopId = price.ShopId;
        existingPrice.PriceDate = price.PriceDate;

        db.Prices.Update(existingPrice);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully updated price {PriceId}.", price.Id);

        return true;
    }

    public async Task<bool> DeletePriceAsync(Guid priceId)
    {
        var price = await db.Prices.FindAsync(priceId);

        if (price == null)
        {
            logger.LogWarning("DeletePriceAsync failed: Price {PriceId} not found.", priceId);
            throw new Exception("Price not found.");
        }

        price.IsDeleted = true;
        price.DeletedDate = DateTime.UtcNow;

        db.Prices.Update(price);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully soft-deleted price {PriceId}.", priceId);

        return true;
    }

    public async Task<PromotionPrice> AddPromotionPriceAsync(PromotionPrice promotionPrice)
    {
        ValidateAmount(promotionPrice.Amount);

        if (promotionPrice.EndDate <= DateTime.UtcNow)
        {
            logger.LogWarning("AddPromotionPriceAsync failed: Promotion end date {EndDate} is not in the future.", promotionPrice.EndDate);
            throw new Exception("Promotion end date must be in the future.");
        }

        if (promotionPrice.StartDate >= promotionPrice.EndDate)
        {
            logger.LogWarning("AddPromotionPriceAsync failed: Promotion start date {StartDate} is after end date {EndDate}.", promotionPrice.StartDate, promotionPrice.EndDate);
            throw new Exception("Promotion start date must be before the end date.");
        }

        var basePriceExists = await db.Prices.AnyAsync(p => p.Id == promotionPrice.PriceId);
        if (!basePriceExists && promotionPrice.Price == null)
        {
            logger.LogWarning("AddPromotionPriceAsync failed: Base price {PriceId} not found.", promotionPrice.PriceId);
            throw new Exception("Base price not found. Cannot attach promotion.");
        }

        await db.Set<PromotionPrice>().AddAsync(promotionPrice);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully added promotion price {PromotionPriceId} to base price {PriceId}.", promotionPrice.Id, promotionPrice.PriceId);

        return promotionPrice;
    }

    public async Task<bool> DeletePromotionPriceAsync(Guid promotionPriceId)
    {
        var promo = await db.Set<PromotionPrice>().FindAsync(promotionPriceId);

        if (promo == null)
        {
            logger.LogWarning("DeletePromotionPriceAsync failed: Promotion {PromotionPriceId} not found.", promotionPriceId);
            throw new Exception("Promotion not found.");
        }

        promo.IsDeleted = true;
        promo.DeletedDate = DateTime.UtcNow;

        db.Set<PromotionPrice>().Update(promo);
        await db.SaveChangesAsync();

        logger.LogInformation("Successfully soft-deleted promotion price {PromotionPriceId}.", promotionPriceId);

        return true;
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            logger.LogWarning("Validation failed: Amount {Amount} is less than or equal to 0.", amount);
            throw new Exception("Amount should be greater than 0.");
        }
    }
}