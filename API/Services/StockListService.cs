using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public interface IStockListService
    {
        Task CreateStockList(Guid userId, string stockListName);
        Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId);
        Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId);
        Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId);
        Task<bool> RemoveUser(Guid stockListId, Guid newUserId, Guid currentUserId);
        Task AddOrUpdateProduct(Guid stockListId,StockListProduct product, Guid currentUserId);
        Task<bool> RemoveProduct(Guid stockListId, Guid productId, Guid currentUserId);
    }
    public class StockListService: IStockListService
    {
        private readonly DatabaseContext db;
        public StockListService(DatabaseContext db) { 
            this.db = db;
        }

        public async Task AddOrUpdateProduct(Guid stockListId, StockListProduct product, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }

            var userStockList = db.UserStockLists
                .FirstOrDefault(us => us.StockListId == stockListId && us.UserId == currentUserId && us.IsActive);

            if (userStockList == null)
            {
                //Or maybe create a new list and add product to it?
                throw new Exception("User cannot add product to this list.");
            }

            var stockListProduct = db.StockListProducts.FirstOrDefault(sp => sp.ProductId == product.Id && sp.StockListId == stockListId);

            if (stockListProduct == null)
            {
                db.StockListProducts.Add(product);
            }
            else
            {
                stockListProduct.Quantity = product.Quantity;
                stockListProduct.ModifiedById = product.ModifiedById;
            }

            await db.SaveChangesAsync();
        }
        public async Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }

            var userStockList =  db.UserStockLists
                .FirstOrDefault(us => us.StockListId == stockListId && us.UserId == newUserId);

            if (userStockList != null)
            {
                if (userStockList.IsActive)
                {
                    throw new Exception("User already has access to list.");
                }
                else
                {
                    userStockList.IsActive = true;
                    userStockList.ModifiedById = currentUserId;
                }
            }
            else
            {
                var newUserStockList = new UserStockList
                {
                    StockListId = stockListId,
                    UserId = newUserId,
                    AddedById = currentUserId,
                    IsActive = true // Set IsActive to true directly
                };
                db.UserStockLists.Add(newUserStockList);
            }

            await db.SaveChangesAsync();
            return true;
        }

        public async Task CreateStockList(Guid userId, string stockListName)
        {
            var stockList = new StockList()
            {
                CreatedById = userId,
                CreatorId = userId,
                Name = stockListName
            };

            db.StockLists.Add(stockList);

            await db.SaveChangesAsync();
        }

        public async Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found");
            }

            if (existingStockList.CreatorId != currentUserId)
            {
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.IsDeleted = true;
            existingStockList.DeletedById = currentUserId;
            existingStockList.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> RemoveProduct(Guid stockListId, Guid productId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }

            var userStockList = db.UserStockLists
                .FirstOrDefault(us => us.StockListId == stockListId && us.UserId == currentUserId && us.IsActive);

            if (userStockList == null)
            {
                //Or maybe create a new list and add product to it?
                throw new Exception("User cannot add product to this list.");
            }

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

        public async Task<bool> RemoveUser(Guid stockListId, Guid removedUserId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }

            if (existingStockList.CreatorId != currentUserId && removedUserId != currentUserId) {
                throw new Exception("User is not allowed to perform operation. You can only remove yourself or remove others from a list you have created.");
            }

            var userStockList = db.UserStockLists.FirstOrDefault(us => us.StockListId == stockListId && us.UserId == removedUserId);

            if (userStockList == null)
            {
                throw new Exception("User not found.");
            }

            userStockList.IsActive = false;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found");
            }

            if (existingStockList.CreatorId != userId)
            {
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.Name = stockListName;

            await db.SaveChangesAsync();

            return true;
        }
    }
}
