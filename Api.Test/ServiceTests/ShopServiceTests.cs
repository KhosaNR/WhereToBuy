using API.Models;
using ApiTest;

namespace ApiTest
{
    [TestClass]
    public class ShopServiceTests : BaseTestClass
    {
        [TestMethod]
        public async Task AddShop_NewValidShop_NewShopIsAdded()
        {
            var shop = new Shop()
            {
                Name = "Test Shop",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            await ShopService.AddShopAsync(shop);

            var savedShop = db.Shops.FirstOrDefault();

            Assert.IsNotNull(savedShop);
            Assert.AreEqual("Test Shop", savedShop.Name, "Shop names are the same.");
            Assert.IsNotNull(savedShop.CreatedDate, "Created date is not null.");
            Assert.IsTrue(savedShop.CreatedDate > DateTime.MinValue, "Created date is not minimum date.");
        }

        [TestMethod]
        public async Task AddShop_DuplicateName_ThrowsException()
        {
            var shop1 = new Shop()
            {
                Name = "Test Shop",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            var shop2 = new Shop()
            {
                Name = "Test Shop",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            await ShopService.AddShopAsync(shop1);

            await Assert.ThrowsExceptionAsync<Exception>(async () => await ShopService.AddShopAsync(shop2), "A shop with the same name already exists.");
        }

        [TestMethod]
        public async Task AddShop_EmptyName_ThrowsException()
        {
            var shop = new Shop()
            {
                Name = "",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await ShopService.AddShopAsync(shop), "Shop name cannot be empty.");
        }

        [TestMethod]
        public async Task UpdateShop_ValidShop_ShopIsUpdated()
        {
            var shop = new Shop()
            {
                Name = "Test Shop",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            await ShopService.AddShopAsync(shop);

            shop.Name = "Updated Test Shop";

            await ShopService.UpdateShopAsync(shop);

            var savedShop = db.Shops.FirstOrDefault();

            Assert.IsNotNull(savedShop);
            Assert.AreEqual("Updated Test Shop", savedShop.Name, "Shop names are the same.");
        }

        [TestMethod]
        public async Task DeleteShop_ValidShop_ShopIsSoftDeleted()
        {
            var shop = new Shop()
            {
                Name = "Test Shop",
                Location = new Location()
                {
                    Link = "https://www.testshop.com",
                    Address = "123 Main St",
                    Longitude = 10.12345,
                    Latitude = 20.67890
                },
                LocationId = Guid.NewGuid()
            };

            await ShopService.AddShopAsync(shop);

            await ShopService.DeleteShopAsync(shop.Id);

            var savedShop = db.Shops.FirstOrDefault();

            Assert.IsNull(savedShop,"Shop is soft deleted");
        }
    }
}