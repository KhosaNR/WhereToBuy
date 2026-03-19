using API.Models;
using API.Models.Dtos;
using API.Models.PriceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IPriceService
{
    Task<CursorPagedResult<Price>> GetAllPricesAsync(int pageSize, DateTime? cursor);

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

    public async Task<CursorPagedResult<Price>> GetAllPricesAsync(int pageSize, DateTime? cursor)
    {
        logger.LogInformation("Retrieving paged prices. PageSize: {PageSize}, Cursor: {Cursor}", pageSize, cursor);

        var query = db.Prices.AsNoTracking()
            .Include(p => p.PromotionPrices)
            .OrderByDescending(p => p.CreatedDate)
            .AsQueryable();

        if (cursor.HasValue)
        {
            query = query.Where(p => p.CreatedDate < cursor.Value);
        }

        var items = await query.Take(pageSize + 1).ToListAsync();
        var hasNextPage = items.Count > pageSize;
        var resultItems = hasNextPage ? items.Take(pageSize).ToList() : items;
        var nextCursor = hasNextPage ? resultItems.Last().CreatedDate : (DateTime?)null;

        logger.LogInformation("Retrieved {Count} prices. Next cursor: {NextCursor}", resultItems.Count, nextCursor);

        return new CursorPagedResult<Price> { Data = resultItems, NextCursor = nextCursor };
    }

    public async Task<Price?> GetPriceAsync(Guid id)
    {
        logger.LogInformation("Fetching price for ID: {PriceId}", id);
        return await db.Prices.AsNoTracking()
            .Include(p => p.PromotionPrices)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Price>> GetPricesAsync(bool activePromotionsOnly = false)
    {
        logger.LogInformation("Fetching prices. ActivePromotionsOnly: {ActiveOnly}", activePromotionsOnly);
        var query = db.Prices.AsNoTracking().AsQueryable();
        if (activePromotionsOnly)
        {
            var now = DateTime.UtcNow;
            query = query.Include(p => p.PromotionPrices!.Where(pp => pp.StartDate <= now && pp.EndDate >= now));
        }
        else
        {
            query = query.Include(p => p.PromotionPrices);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Price>> GetPricesByShopIdAsync(Guid shopId)
    {
        logger.LogInformation("Fetching prices for Shop: {ShopId}", shopId);
        return await db.Prices.AsNoTracking()
            .Include(p => p.Shop).Include(p => p.PromotionPrices)
            .Where(p => p.ShopId == shopId).ToListAsync();
    }

    public async Task<List<Price>> GetPricesByProductIdAsync(Guid productId)
    {
        logger.LogInformation("Fetching prices for Product: {ProductId}", productId);
        return await db.Prices.AsNoTracking()
            .Include(p => p.Product).Include(p => p.PromotionPrices)
            .Where(p => p.ProductId == productId).ToListAsync();
    }

    public async Task<Price> AddPriceAsync(Price price)
    {
        logger.LogInformation("Adding new price for Product {ProductId} at Shop {ShopId}", price.ProductId, price.ShopId);
        ValidateAmount(price.Amount);

        var existingPrice = await db.Prices.FirstOrDefaultAsync(p => p.ProductId == price.ProductId && p.ShopId == price.ShopId);
        if (existingPrice != null)
        {
            logger.LogWarning("Price entry already exists for Product {ProductId} at Shop {ShopId}", price.ProductId, price.ShopId);
            throw new Exception("A base price already exists for this product at this shop.");
        }

        await db.Prices.AddAsync(price);
        await db.SaveChangesAsync();

        logger.LogInformation("Price successfully added with ID: {PriceId}", price.Id);
        return price;
    }

    public async Task<bool> UpdatePriceAsync(Price price)
    {
        logger.LogInformation("Updating price ID: {PriceId}", price.Id);
        ValidateAmount(price.Amount);

        var existingPrice = await db.Prices.FindAsync(price.Id);
        if (existingPrice == null)
        {
            logger.LogWarning("Update failed. Price ID: {PriceId} not found", price.Id);
            throw new Exception("Price not found.");
        }

        existingPrice.Amount = price.Amount;
        existingPrice.Url = price.Url;
        existingPrice.PriceDate = price.PriceDate;

        db.Prices.Update(existingPrice);
        await db.SaveChangesAsync();

        logger.LogInformation("Price ID: {PriceId} updated successfully", price.Id);
        return true;
    }

    public async Task<bool> DeletePriceAsync(Guid priceId)
    {
        logger.LogInformation("Soft-deleting price ID: {PriceId}", priceId);
        var price = await db.Prices.FindAsync(priceId);
        if (price == null)
        {
            logger.LogWarning("Delete failed. Price ID: {PriceId} not found", priceId);
            throw new Exception("Price not found.");
        }

        price.IsDeleted = true;
        price.DeletedDate = DateTime.UtcNow;

        await db.SaveChangesAsync();
        logger.LogInformation("Price ID: {PriceId} soft-deleted successfully", priceId);
        return true;
    }

    public async Task<PromotionPrice> AddPromotionPriceAsync(PromotionPrice promotionPrice)
    {
        logger.LogInformation("Adding promotion to price ID: {PriceId}", promotionPrice.PriceId);
        ValidateAmount(promotionPrice.Amount);

        if (promotionPrice.EndDate <= DateTime.UtcNow)
        {
            logger.LogWarning("Promotion failed. EndDate is in the past: {EndDate}", promotionPrice.EndDate);
            throw new Exception("Promotion end date must be in the future.");
        }

        await db.Set<PromotionPrice>().AddAsync(promotionPrice);
        await db.SaveChangesAsync();

        logger.LogInformation("Promotion added successfully with ID: {PromoId}", promotionPrice.Id);
        return promotionPrice;
    }

    public async Task<bool> DeletePromotionPriceAsync(Guid promotionPriceId)
    {
        logger.LogInformation("Soft-deleting promotion ID: {PromoId}", promotionPriceId);
        var promo = await db.Set<PromotionPrice>().FindAsync(promotionPriceId);
        if (promo == null)
        {
            logger.LogWarning("Delete failed. Promotion ID: {PromoId} not found", promotionPriceId);
            throw new Exception("Promotion not found.");
        }

        promo.IsDeleted = true;
        promo.DeletedDate = DateTime.UtcNow;

        await db.SaveChangesAsync();
        logger.LogInformation("Promotion ID: {PromoId} soft-deleted successfully", promotionPriceId);
        return true;
    }

    private void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            logger.LogWarning("Validation failed. Amount must be positive: {Amount}", amount);
            throw new Exception("Amount should be greater than 0.");
        }
    }
}