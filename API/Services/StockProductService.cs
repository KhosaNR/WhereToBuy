using API.Models;
using System.Drawing.Printing;

namespace API.Services
{
    public interface IStockProductService
    {
        Task AddOrUpdateProduct(Guid stockListId, StockListProduct product, Guid currentUserId);
        Task<bool> RemoveProduct(Guid stockListId, Guid productId, Guid currentUserId);
    }
    public class StockProductService
    {
        private readonly DatabaseContext db;

        public StockProductService(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task AddOrUpdateProduct(Guid stockListId, StockListProduct product, Guid currentUserId)
        {
            await ValidateProductExist(product.ProductId);

            if (product.StockListId != Guid.Empty && product.StockListId != stockListId)
            {
                throw new Exception("Product linked to a different list");
            }
            await ValidateStockListExist(stockListId);

            ValidateUserCanEditStockList(stockListId, currentUserId);

            var stockListProduct = db.StockListProducts.FirstOrDefault(sp => sp.Id == product.Id && sp.StockListId == stockListId);

            if (stockListProduct == null)
            {
                product.StockListId = stockListId;
                db.StockListProducts.Add(product);
            }
            else
            {
                stockListProduct.Quantity = product.Quantity;
                stockListProduct.ModifiedById = product.ModifiedById;
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
                throw new Exception("Product not found.");
            }

            stockListProduct.IsDeleted = true;
            stockListProduct.DeletedById = currentUserId;
            stockListProduct.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return true;
        }

        private void ValidateUserCanEditStockList(Guid stockListId, Guid currentUserId)
        {
            var ownsTheStockList = db.StockLists.Any(s => s.Id == stockListId && s.OwnerId == currentUserId);

            var userHasAccessToStockList = db.UserStockLists
                .Any(us => us.StockListId == stockListId && us.UserId == currentUserId && us.IsActive);

            if (!ownsTheStockList && !userHasAccessToStockList)
            {
                //Or maybe create a new list and add product to it?
                throw new Exception("User add, update or remove product to this list.");
            }
        }

        private async Task ValidateStockListExist(Guid stockListId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }
        }

        private async Task ValidateProductExist(Guid productId)
        {
            if(productId == Guid.Empty)
            {
                throw new Exception("ProductId is required.");
            }

            var existingStockList = await db.Products.FindAsync(productId);
            if (existingStockList == null)
            {
                throw new Exception("Product not found.");
            }
        }
    }
}
