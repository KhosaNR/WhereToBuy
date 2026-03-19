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
    public class StockProductServiceTests : BaseTestClass
    {
        /*
        [TestMethod]
        public async Task AddOrUpdateProduct_ValidProduct_ProductIsAdded()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync((new User { Id = userId, Username = "Test user" }));
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1 };

            Act
           await StockListService.AddOrUpdateProduct(stockList.Id, product, userId);

            Assert
           var savedProduct = db.StockListProducts.FirstOrDefault(sp => sp.StockListId == stockList.Id && sp.ProductId == product.Id);
            Assert.IsNotNull(savedProduct);
            Assert.AreEqual(product.Quantity, savedProduct.Quantity);
        }

        [TestMethod]
        public async Task AddOrUpdateProduct_ValidExistingProduct_ProductIsUpdated()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync((new User { Id = userId, Username = "Test user" }));
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1 };
            await StockListService.AddOrUpdateProduct(stockList.Id, product, userId);
            product.Quantity = 2;

            Act
           await StockListService.AddOrUpdateProduct(stockList.Id, product, userId);

            Assert
           var savedProduct = db.StockListProducts.FirstOrDefault(sp => sp.StockListId == stockList.Id && sp.ProductId == product.Id);
            Assert.IsNotNull(savedProduct);
            Assert.AreEqual(product.Quantity, savedProduct.Quantity);
        }

        [TestMethod]
        public async Task AddOrUpdateProduct_NonExistentStockList_ThrowsException()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1 };

            Act and Assert
           await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.AddOrUpdateProduct(stockListId, product, userId), "List not found.");
        }

        [TestMethod]
        public async Task AddOrUpdateProduct_UnauthorizedUser_ThrowsException()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync(new User { Id = userId, Username = "Unauthorized user" });
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1 };
            var unauthorizedUserId = Guid.NewGuid();

            Act and Assert
           await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.AddOrUpdateProduct(stockList.Id, product, unauthorizedUserId), "User cannot add product to this list.");
        }

        [TestMethod]
        public async Task RemoveProduct_ValidProduct_ProductIsRemoved()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync(new User { Id = userId, Username = "Test user" });
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1, ProductId = Guid.NewGuid(), StockListId = stockList.Id };
            await StockListService.AddOrUpdateProduct(stockList.Id, product, userId);

            Act
           await StockListService.RemoveProduct(stockList.Id, product.ProductId, userId);

            Assert
           var savedProduct = db.StockListProducts.FirstOrDefault(sp => sp.StockListId == stockList.Id && sp.ProductId == product.Id);
            Assert.IsNull(savedProduct);
        }

        [TestMethod]
        public async Task RemoveProduct_NonExistentStockList_ThrowsException()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            Act and Assert
           await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveProduct(stockListId, productId, userId), "List not found.");
        }

        [TestMethod]
        public async Task RemoveProduct_NonExistentProduct_ThrowsException()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync((new User { Id = userId, Username = "Test user" }));
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var productId = Guid.NewGuid();

            Act and Assert
           await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveProduct(stockList.Id, productId, userId), "Product not found.");
        }

        [TestMethod]
        public async Task RemoveProduct_UnauthorizedUser_ThrowsException()
        {
            Arrange
           var userId = Guid.NewGuid();
            var stockListName = "Test Stock List";
            await db.Users.AddAsync((new User { Id = userId, Username = "Test user" }));
            await db.SaveChangesAsync();
            await StockListService.CreateStockList(userId, stockListName);
            var stockList = db.StockLists.FirstOrDefault(s => s.OwnerId == userId && s.Name == stockListName);
            var product = new StockListProduct { Id = Guid.NewGuid(), Quantity = 1 };
            await StockListService.AddOrUpdateProduct(stockList.Id, product, userId);
            var unauthorizedUserId = Guid.NewGuid();

            Act and Assert
           await Assert.ThrowsExceptionAsync<Exception>(async () => await StockListService.RemoveProduct(stockList.Id, product.Id, unauthorizedUserId), "User cannot add product to this list.");
        }*/
    }
}
