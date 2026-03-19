using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;
using ApiTest;

namespace Api.Test.ServiceTests
{
    [TestClass]
    public class StockListServiceTests : BaseTestClass
    {
        // private StockListProduct stockListProduct;
        // private StockList stockList;
        private User guestUser;

        [TestInitialize]
        public void Initialize()
        {
            guestUser = new Guest()
            {
                Username = "Guest User",
            };

            Db.Users.Add(guestUser);
            Db.SaveChanges();
        }

        [TestMethod]
        public async Task CreateStockList_ValidStockUserId_CreatesStockList()
        {
            await StockListService.CreateStockList(guestUser.Id, string.Empty);

            var stockList = Db.StockLists.FirstOrDefault();

            Assert.IsNotNull(stockList, "Stock list is not null");
            Assert.AreEqual(guestUser.Id, stockList.OwnerId, "Stock list owner is guest.");
        }

        [TestMethod]
        public async Task CreateStockList_DuplicateStockListName_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.CreateStockList(userId, stockListName), $"User already has a stock list named {stockListName}");
        }

        [TestMethod]
        public async Task CreateStockList_NonExistentUser_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.CreateStockList(userId, stockListName), "User not found.");
        }

        [TestMethod]
        public async Task DeleteStockList_ValidStockList_StockListIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);

            // Act
            await StockListService.DeleteStockList(stockList.Id, userId);

            // Assert
            var savedStockList = Db.StockLists.FirstOrDefault(s => s.Id == stockList.Id);
            Assert.IsNull(savedStockList);
        }

        [TestMethod]
        public async Task DeleteStockList_NonExistentStockList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.DeleteStockList(stockListId, userId), "List not found");
        }

        [TestMethod]
        public async Task DeleteStockList_UnauthorizedUser_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var unauthorizedUserId = Guid.NewGuid();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.DeleteStockList(stockList.Id, unauthorizedUserId), "User is not allowed to perform operation. Only owner of list can update this information.");
        }

        [TestMethod]
        public async Task UpdateStockList_ValidStockList_StockListIsUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newStockListName = "Updated Test Stock List";

            // Act
            await StockListService.UpdateStockList(stockList.Id, newStockListName, userId);

            // Assert
            var savedStockList = Db.StockLists.FirstOrDefault(s => s.Id == stockList.Id);
            Assert.IsNotNull(savedStockList);
            Assert.AreEqual(newStockListName, savedStockList.Name);
        }

        [TestMethod]
        public async Task UpdateStockList_NonExistentStockList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();
            var newStockListName = "Updated Test Stock List";

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.UpdateStockList(stockListId, newStockListName, userId), "List not found.");
        }

        [TestMethod]
        public async Task AddUser_ValidUser_UserIsAdded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();

            // Act
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);

            // Assert
            var userStockList = Db.UserStockLists.FirstOrDefault(us => us.StockListId == stockList.Id && us.UserId == newUser.Id);
            Assert.IsNotNull(userStockList);
            Assert.IsTrue(userStockList.IsActive);
        }

        [TestMethod]
        public async Task AddUser_NonExistentStockList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.AddUser(stockListId, newUser.Id, userId), "List not found.");
        }

        [TestMethod]
        public async Task AddUser_UserAlreadyHasAccessToStockList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.AddUser(stockList.Id, newUser.Id, userId), "User already has access to list.");
        }

        [TestMethod]
        public async Task AddUser_PreviouslyRemovedUser_UserIsReAdded()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);
            var userStockList = Db.UserStockLists.FirstOrDefault(us => us.StockListId == stockList.Id && us.UserId == newUser.Id);
            userStockList.IsActive = false;
            await Db.SaveChangesAsync();

            // Act
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);

            // Assert
            var savedUserStockList = Db.UserStockLists.FirstOrDefault(us => us.StockListId == stockList.Id && us.UserId == newUser.Id);
            Assert.IsNotNull(savedUserStockList);
            Assert.IsTrue(savedUserStockList.IsActive);
        }

        [TestMethod]
        public async Task RemoveUser_ValidUser_UserIsRemoved()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);

            // Act
            await StockListService.RemoveUser(stockList.Id, newUser.Id, userId);

            // Assert
            var userStockList = Db.UserStockLists.FirstOrDefault(us => us.StockListId == stockList.Id && us.UserId == newUser.Id);
            Assert.IsNotNull(userStockList);
            Assert.IsFalse(userStockList.IsActive);
        }

        [TestMethod]
        public async Task RemoveUser_NonExistentStockList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();
            var removedUserId = Guid.NewGuid();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveUser(stockListId, removedUserId, userId), "List not found.");
        }

        [TestMethod]
        public async Task RemoveUser_UnauthorizedUser_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();
            await StockListService.AddUser(stockList.Id, newUser.Id, userId);
            var unauthorizedUserId = Guid.NewGuid();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveUser(stockList.Id, newUser.Id, unauthorizedUserId), "User is not allowed to perform operation. You can only remove yourself or remove others from a list you have created.");
        }

        [TestMethod]
        public async Task RemoveUser_UserNotLinkedToList_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var newUser = new User { Id = Guid.NewGuid() };
            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveUser(stockList.Id, newUser.Id, userId), "User not linked to list.");
        }

        [TestMethod]
        public async Task RemoveUser_RemoveYourselfFromList_UserIsRemoved()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await Db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await Db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = Db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);

            // Act
            await StockListService.RemoveUser(stockList.Id, userId, userId);

            // Assert
            var userStockList = Db.UserStockLists.FirstOrDefault(us => us.StockListId == stockList.Id && us.UserId == userId);
            Assert.IsNotNull(userStockList);
            Assert.IsFalse(userStockList.IsActive);
        }
    }
}
