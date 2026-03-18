using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using Microsoft.Extensions.Logging;

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

    public class StockListService : IStockListService
    {
        private readonly DatabaseContext db;
        private readonly ILogger<StockListService> logger;

        public StockListService(DatabaseContext db, ILogger<StockListService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId)
        {
            await ValidateStockListExist(stockListId);

            var userStockList = db.UserStockLists
                .FirstOrDefault(us => us.StockListId == stockListId && us.UserId == newUserId);

            if (userStockList != null)
            {
                if (userStockList.IsActive)
                {
                    logger.LogWarning("AddUser failed: User {NewUserId} already has active access to stock list {StockListId}.", newUserId, stockListId);
                    throw new Exception("User already has access to list.");
                }
                else
                {
                    userStockList.IsActive = true;
                    userStockList.ModifiedById = currentUserId;
                    logger.LogInformation("Reactivated access for User {NewUserId} to stock list {StockListId}.", newUserId, stockListId);
                }
            }
            else
            {
                var newUserStockList = new UserStockList
                {
                    StockListId = stockListId,
                    UserId = newUserId,
                    AddedById = currentUserId,
                    IsActive = true
                };
                db.UserStockLists.Add(newUserStockList);
                logger.LogInformation("Granted access for User {NewUserId} to stock list {StockListId}.", newUserId, stockListId);
            }

            await db.SaveChangesAsync();
            return true;
        }

        public async Task CreateStockList(Guid userId, string stockListName)
        {
            var userExist = db.Users.Any(u => u.Id == userId);
            if (!userExist)
            {
                logger.LogWarning("CreateStockList failed: User {UserId} not found.", userId);
                throw new Exception("User not found.");
            }

            var userHasStockListName = db.StockLists.Any(s => s.OwnerId == userId && s.Name == stockListName);
            if (userHasStockListName)
            {
                logger.LogWarning("CreateStockList failed: User {UserId} already owns a list named '{StockListName}'.", userId, stockListName);
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

            logger.LogInformation("Successfully created stock list '{StockListName}' ({StockListId}) for User {UserId}.", stockListName, stockListId, userId);
        }

        public async Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                logger.LogWarning("DeleteStockList failed: Stock list {StockListId} not found.", stockListId);
                throw new Exception("List not found");
            }

            if (existingStockList.OwnerId != currentUserId)
            {
                logger.LogWarning("DeleteStockList failed: User {UserId} is not the owner of list {StockListId}.", currentUserId, stockListId);
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.IsDeleted = true;
            existingStockList.DeletedById = currentUserId;
            existingStockList.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully soft-deleted stock list {StockListId}.", stockListId);

            return true;
        }

        public async Task<bool> RemoveUser(Guid stockListId, Guid removedUserId, Guid currentUserId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                logger.LogWarning("RemoveUser failed: Stock list {StockListId} not found.", stockListId);
                throw new Exception("List not found.");
            }

            if (existingStockList.OwnerId != currentUserId && removedUserId != currentUserId)
            {
                logger.LogWarning("RemoveUser failed: User {UserId} attempted to remove User {RemovedUserId} without owner privileges on list {StockListId}.", currentUserId, removedUserId, stockListId);
                throw new Exception("User is not allowed to perform operation. You can only remove yourself or remove others from a list you have created.");
            }

            var userStockList = db.UserStockLists.FirstOrDefault(us => us.StockListId == stockListId && us.UserId == removedUserId);

            if (userStockList == null)
            {
                logger.LogWarning("RemoveUser failed: User {RemovedUserId} is not linked to list {StockListId}.", removedUserId, stockListId);
                throw new Exception("User not linked to list.");
            }

            userStockList.IsActive = false;
            await db.SaveChangesAsync();

            logger.LogInformation("Successfully removed User {RemovedUserId} from stock list {StockListId}.", removedUserId, stockListId);

            return true;
        }

        public async Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId)
        {
            var existingStockList = await db.StockLists.FindAsync(stockListId);
            if (existingStockList == null)
            {
                logger.LogWarning("UpdateStockList failed: Stock list {StockListId} not found.", stockListId);
                throw new Exception("List not found.");
            }

            if (existingStockList.OwnerId != userId)
            {
                logger.LogWarning("UpdateStockList failed: User {UserId} is not the owner of list {StockListId}.", userId, stockListId);
                throw new Exception("User is not allowed to perform operation. Only owner of list can update this information.");
            }

            existingStockList.Name = stockListName;

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully updated name of stock list {StockListId} to '{StockListName}'.", stockListId, stockListName);

            return true;
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
    }
}