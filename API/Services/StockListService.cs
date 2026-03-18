using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;

namespace API.Services
{
    public interface IStockListService
    {
        Task CreateStockList(Guid userId, string stockListName);
        Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId);
        Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId);
        Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId);
        Task<bool> RemoveUser(Guid stockListId, Guid newUserId, Guid currentUserId);
    }
    public class StockListService: IStockListService
    {
        private readonly DatabaseContext db;
        public StockListService(DatabaseContext db) { 
            this.db = db;
        }

        public async Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId)
        {
            await ValidateStockListExist(stockListId);

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
            var userExist = db.Users.Any( u => u.Id == userId);
            if (!userExist)
            {
                throw new Exception("User not found.");
            }

            var userHasStockListName = db.StockLists.Any(s => s.OwnerId == userId && s.Name == stockListName);
            if (userHasStockListName)
            {
                throw new Exception($"User already has a stock list named {stockListName}");
            }

            var stockListId = Guid.NewGuid();
            var stockList = new StockList()
            {
                Id = stockListId,
                CreatedById = userId,
                OwnerId = userId,
                Name = stockListName
            };

            db.StockLists.Add(stockList);

            var userStockList = new UserStockList()
            {
                AddedById = userId,
                UserId = userId,
                StockListId = stockListId,
            };

            db.UserStockLists.Add(userStockList);

            await db.SaveChangesAsync();
        }

        public async Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found");
            }

            if (existingStockList.OwnerId != currentUserId)
            {
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.IsDeleted = true;
            existingStockList.DeletedById = currentUserId;
            existingStockList.DeletedDate = DateTime.UtcNow;

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

            if (existingStockList.OwnerId != currentUserId && removedUserId != currentUserId) {
                throw new Exception("User is not allowed to perform operation. You can only remove yourself or remove others from a list you have created.");
            }

            var userStockList = db.UserStockLists.FirstOrDefault(us => us.StockListId == stockListId && us.UserId == removedUserId);

            if (userStockList == null)
            {
                throw new Exception("User not linked to list.");
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
                throw new Exception("List not found.");
            }

            if (existingStockList.OwnerId != userId)
            {
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.Name = stockListName;

            await db.SaveChangesAsync();

            return true;
        }

        private async Task ValidateStockListExist(Guid stockListId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                throw new Exception("List not found.");
            }
        }
    }
}
