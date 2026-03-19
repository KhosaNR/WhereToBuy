namespace API.Services
{
    using API.Models;
    using API.Models.Dtos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public interface IStockListService
    {
        Task<CursorPagedResult<StockList>> GetAllStockListsAsync(int pageSize, DateTime? cursor, Guid userId);

        Task<StockList?> GetStockListAsync(Guid id, Guid userId);

        Task CreateStockList(Guid userId, string stockListName);

        Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId);

        Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId);

        Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId);

        Task<bool> RemoveUser(Guid stockListId, Guid removedUserId, Guid currentUserId);
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

        public async Task<CursorPagedResult<StockList>> GetAllStockListsAsync(int pageSize, DateTime? cursor, Guid userId)
        {
            var query = db.StockLists.AsNoTracking()
                .Where(sl => sl.OwnerId == userId || sl.SharedUsers.Any(su => su.UserId == userId && su.IsActive))
                .OrderByDescending(sl => sl.CreatedDate)
                .AsQueryable();

            if (cursor.HasValue)
            {
                query = query.Where(sl => sl.CreatedDate < cursor.Value);
            }

            var items = await query.Take(pageSize + 1).ToListAsync();
            var hasNextPage = items.Count > pageSize;
            var resultItems = hasNextPage ? items.Take(pageSize).ToList() : items;
            var nextCursor = hasNextPage ? resultItems.Last().CreatedDate : (DateTime?)null;

            return new CursorPagedResult<StockList> { Data = resultItems, NextCursor = nextCursor };
        }

        public async Task<StockList?> GetStockListAsync(Guid id, Guid userId)
        {
            return await db.StockLists.AsNoTracking()
                .Include(sl => sl.StockListProducts)
                .FirstOrDefaultAsync(sl => sl.Id == id && (sl.OwnerId == userId || sl.SharedUsers.Any(su => su.UserId == userId && su.IsActive)));
        }

        public async Task CreateStockList(Guid userId, string stockListName)
        {
            var userExists = await db.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new Exception("User not found.");
            }

            if (db.StockLists.Any(s => s.OwnerId == userId && s.Name == stockListName))
            {
                throw new Exception($"User already has a stock list named {stockListName}");
            }

            var stockList = new StockList { Id = Guid.NewGuid(), OwnerId = userId, Name = stockListName, CreatedById = userId };
            db.StockLists.Add(stockList);
            db.UserStockLists.Add(new UserStockList { StockListId = stockList.Id, UserId = userId, AddedById = userId, IsActive = true });
            await db.SaveChangesAsync();
        }

        public async Task<bool> UpdateStockList(Guid stockListId, string stockListName, Guid userId)
        {
            var list = await db.StockLists.FindAsync(stockListId);
            if (list == null || list.OwnerId != userId)
            {
                throw new Exception("Unauthorized or list not found.");
            }

            list.Name = stockListName;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStockList(Guid stockListId, Guid currentUserId)
        {
            var list = await db.StockLists.FindAsync(stockListId);
            if (list == null || list.OwnerId != currentUserId)
            {
                throw new Exception("Unauthorized or list not found.");
            }

            list.IsDeleted = true;
            list.DeletedDate = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUser(Guid stockListId, Guid newUserId, Guid currentUserId)
        {
            var list = await db.StockLists.FindAsync(stockListId);
            if (list == null || list.OwnerId != currentUserId)
            {
                throw new Exception("Unauthorized.");
            }

            var exists = await db.UserStockLists.FirstOrDefaultAsync(us => us.StockListId == stockListId && us.UserId == newUserId);
            if (exists != null)
            {
                if (exists.IsActive)
                {
                    throw new Exception("User is already added to the stock list.");
                }

                exists.IsActive = true;
            }
            else
            {
                db.UserStockLists.Add(new UserStockList { StockListId = stockListId, UserId = newUserId, AddedById = currentUserId, IsActive = true });
            }

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUser(Guid stockListId, Guid removedUserId, Guid currentUserId)
        {
            var list = await db.StockLists.FindAsync(stockListId);
            if (list == null || (list.OwnerId != currentUserId && removedUserId != currentUserId))
            {
                throw new Exception("Unauthorized.");
            }

            var link = await db.UserStockLists.FirstOrDefaultAsync(us => us.StockListId == stockListId && us.UserId == removedUserId);
            if (link == null)
            {
                throw new Exception("User is not associated with the stock list.");
            }
            else
            {
                link.IsActive = false;
            }

            await db.SaveChangesAsync();
            return true;
        }
    }
}