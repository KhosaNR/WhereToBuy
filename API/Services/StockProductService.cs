using API.Models;
using System.Drawing.Printing;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public interface IStockProductService
    {
        Task AddOrUpdateProduct(Guid stockListId, StockListProduct product, Guid currentUserId);
        Task<bool> RemoveProduct(Guid stockListId, Guid productId, Guid currentUserId);
    }

    public class StockProductService : IStockProductService
    {
        private readonly DatabaseContext db;
        private readonly ILogger<StockProductService> logger;

        public StockProductService(DatabaseContext db, ILogger<StockProductService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task AddOrUpdateProduct(Guid stockListId, StockListProduct product, Guid currentUserId)
        {
            await ValidateProductExist(product.ProductId);

            if (product.StockListId != Guid.Empty && product.StockListId != stockListId)
            {
                logger.LogWarning("AddOrUpdateProduct failed: Product is linked to list {ProductStockListId}, not target list {StockListId}.", product.StockListId, stockListId);
                throw new Exception("Product linked to a different list");
            }
            await ValidateStockListExist(stockListId);

            ValidateUserCanEditStockList(stockListId, currentUserId);

            var stockListProduct = db.StockListProducts.FirstOrDefault(sp => sp.Id == product.Id && sp.StockListId == stockListId);

            if (stockListProduct == null)
            {
                product.StockListId = stockListId;
                db.StockListProducts.Add(product);
                logger.LogInformation("Added new product {ProductId} to stock list {StockListId}.", product.ProductId, stockListId);
            }
            else
            {
                stockListProduct.Quantity = product.Quantity;
                stockListProduct.ModifiedById = product.ModifiedById;
                logger.LogInformation("Updated product {ProductId} quantity on stock list {StockListId}.", product.ProductId, stockListId);
            }

            await db.SaveChangesAsync();
        }

        public async Task<bool> RemoveProduct(Guid stockListId, Guid productId, Guid currentUserId)
        {
            await ValidateStockListExist(stockListId);

            ValidateUserCanEditStockList(stockListId, currentUserId);

            var stockListProducts = db.StockListProducts.ToList();

            var stockListProduct = db.StockListProducts.FirstOrDefault(sp => sp.ProductId == productId && sp.StockListId == stockListId);

            if (stockListProduct == null)
            {
                logger.LogWarning("RemoveProduct failed: Product {ProductId} not found in stock list {StockListId}.", productId, stockListId);
                throw new Exception("Product not found.");
            }

            stockListProduct.IsDeleted = true;
            stockListProduct.DeletedById = currentUserId;
            stockListProduct.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully soft-deleted product {ProductId} from stock list {StockListId}.", productId, stockListId);

            return true;
        }

        private void ValidateUserCanEditStockList(Guid stockListId, Guid currentUserId)
        {
            var ownsTheStockList = db.StockLists.Any(s => s.Id == stockListId && s.OwnerId == currentUserId);

            var userHasAccessToStockList = db.UserStockLists
                .Any(us => us.StockListId == stockListId && us.UserId == currentUserId && us.IsActive);

            if (!ownsTheStockList && !userHasAccessToStockList)
            {
                logger.LogWarning("Authorization failed: User {UserId} attempted to modify stock list {StockListId} without access.", currentUserId, stockListId);
                throw new Exception("User add, update or remove product to this list.");
            }
        }

        private async Task ValidateStockListExist(Guid stockListId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                logger.LogWarning("Validation failed: Stock list {StockListId} not found.", stockListId);
                throw new Exception("List not found.");
            }
        }

        private async Task ValidateProductExist(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                logger.LogWarning("Validation failed: Product ID is empty.");
                throw new Exception("ProductId is required.");
            }

            var existingStockList = await db.Products.FindAsync(productId);
            if (existingStockList == null)
            {
                logger.LogWarning("Validation failed: Product {ProductId} not found in database.", productId);
                throw new Exception("Product not found.");
            }
        }
    }
}