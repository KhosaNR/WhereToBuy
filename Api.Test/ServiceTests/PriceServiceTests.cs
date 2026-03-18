using API.Models;
using API.Models.PriceModels;

namespace ApiTest
{
    [TestClass]
    public class PriceServiceTests : BaseTestClass
    {
        private Product DefaultProduct { get; set; }
        private Product DefaultProduct2 { get; set; }
        private Shop DefaultShop { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            TestInitialize();
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                QuantityPerUnit = 300,
                Variants = "Original"
            };

            DefaultProduct = product;
            db.Products.Add(DefaultProduct);

            var product2 = new Product()
            {
                Name = "Coca cola",
                Description = "Coca cola zero",
                UnitOfMeasure = new()
                {
                    Name = "Litres",
                    Abbreviation = "l"
                },
                QuantityPerUnit = 2,
                Variants = "Zero"
            };

            DefaultProduct2 = product2;
            db.Products.Add(DefaultProduct2);

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
            DefaultShop = shop;
            db.Shops.Add(DefaultShop);

            db.SaveChanges();

        }

        [TestMethod]
        public async Task GetPriceAsync_ValidId_PriceIsReturned()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price);

            var savedPrice = await PriceService.GetPriceAsync(price.Id);

            Assert.IsNotNull(savedPrice);
            Assert.AreEqual(price.Amount, savedPrice.Amount, "Amounts are the same.");
            Assert.AreEqual(price.Url, savedPrice.Url, "Urls are the same.");
        }

        [TestMethod]
        public async Task GetPricesAsync_PricesAreReturned()
        {
            var price1 = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            var price2 = new Price()
            {
                Amount = 5.99M,
                Url = "https://www.example2.com",
                ProductId = DefaultProduct2.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price1);
            await PriceService.AddPriceAsync(price2);

            var savedPrices = await PriceService.GetPricesAsync();

            Assert.IsNotNull(savedPrices);
            Assert.AreEqual(2, savedPrices.Count, "Two prices are returned.");
        }

        //[TestMethod]
        //public async Task GetPricesByShopIdAsync_ValidShopId_PricesAreReturned()
        //{
        //    var price1 = new Price()
        //    {
        //        Amount = 10.99,
        //        Url = "https://www.example.com",
        //        ProductId = Guid.NewGuid(),
        //        ShopId = shopId
        //    };

        //    var price2 = new Price()
        //    {
        //        Amount = 5.99,
        //        Url = "https://www.example2.com",
        //        ProductId = Guid.NewGuid(),
        //        ShopId = Guid.NewGuid()
        //    };

        //    await PriceService.AddPriceAsync(price1);
        //    await PriceService.AddPriceAsync(price2);

        //    var savedPrices = await PriceService.GetPricesByShopIdAsync(shopId);

        //    Assert.IsNotNull(savedPrices);
        //    Assert.AreEqual(1, savedPrices.Count, "One price is returned.");
        //}

        [TestMethod]
        public async Task GetPricesByProductIdAsync_ValidProductId_PricesAreReturned()
        {
            var price1 = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            var price2 = new Price()
            {
                Amount = 50.99M,
                Url = "https://www.example2.com",
                ProductId = DefaultProduct2.Id,
                ShopId = DefaultShop.Id,
            };

            await PriceService.AddPriceAsync(price1);
            await PriceService.AddPriceAsync(price2);

            var savedPrices = await PriceService.GetPricesByProductIdAsync(DefaultProduct2.Id);

            Assert.IsNotNull(savedPrices);
            Assert.AreEqual(1, savedPrices.Count, "One price is returned.");
        }

        [TestMethod]
        public async Task AddPriceAsync_NewValidPrice_PriceIsAdded()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price);

            var savedPrice = db.Prices.FirstOrDefault();

            Assert.IsNotNull(savedPrice);
            Assert.AreEqual(price.Amount, savedPrice.Amount, "Amounts are the same.");
            Assert.AreEqual(price.Url, savedPrice.Url, "Urls are the same.");
        }

        [TestMethod]
        public async Task AddPriceAsync_NewValidPromotionPrice_PriceIsAdded()
        {
            var price = new Price()
            {
                Amount = 15.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            var promoPrice = new PromotionPrice()
            {
                Amount = 10.99M,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Price = price
            };

            await PriceService.AddPromotionPriceAsync(promoPrice);

            var svP = await PriceService.GetPricesAsync();

            var savedPriceList = await PriceService.GetPricesAsync();
            Assert.IsNotNull(savedPriceList, "Prices are not null.");
            var savedPrice = savedPriceList.FirstOrDefault();

            Assert.IsNotNull(savedPrice);
            Assert.IsNotNull(promoPrice);
            Assert.IsNotNull(savedPrice.PromotionPrices);
            Assert.AreEqual(promoPrice.Price.Amount, savedPrice.Amount, "Amounts are the same.");
            Assert.AreEqual(promoPrice.Amount, savedPrice.PromotionPrices.FirstOrDefault().Amount, "Promotion amounts are the same.");
            Assert.AreEqual(true, promoPrice.IsActive, "Promo price is active");
            Assert.AreEqual(promoPrice.Price.Url, savedPrice.Url, "Urls are the same.");
        }


        [TestMethod]
        public async Task AddPriceAsync_InvalidPromotionPriceEndDate_ThrowException()
        {
            var price = new Price()
            {
                Amount = 15.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            var promoPrice = new PromotionPrice()
            {
                Amount = 10.99M,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1),
                Price = price
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.AddPromotionPriceAsync(promoPrice), "Invalid promotion price end date. Promotion price cannot be less than now.");
        }

        [TestMethod]
        public async Task GetPriceAsync_ExpiredPromotionPrice_PromotionIsNotActive()
        {
            var price = new Price()
            {
                Amount = 15.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            var promoPrice = new PromotionPrice()
            {
                Amount = 10.99M,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Price= price
            };
            await PriceService.AddPromotionPriceAsync(promoPrice);
            promoPrice.EndDate = DateTime.UtcNow.AddDays(-1);
            await db.SaveChangesAsync();

            db.ChangeTracker.Clear();

            var savedPriceList = await PriceService.GetPricesAsync(activePromotionsOnly: true);
            Assert.IsNotNull(savedPriceList, "Prices are not null.");
            var savedPromoPrice = savedPriceList.FirstOrDefault().PromotionPrices;

            Assert.AreEqual(0,savedPromoPrice.Count,"Promotion price is null");
        }

        [TestMethod]
        public async Task AddPriceAsync_PriceWithInvalidAmount_ThrowsException()
        {
            var price = new Price()
            {
                Amount = -1,
                Url = "https://www.example.com",
                ProductId = Guid.NewGuid(),
                ShopId = Guid.NewGuid()
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.AddPriceAsync(price), "Amount should be greater than 0.");
        }
        [TestMethod]
        public async Task GetPriceAsync_InvalidId_NullIsReturned()
        {
            var priceId = Guid.NewGuid();

            var savedPrice = await PriceService.GetPriceAsync(priceId);

            Assert.IsNull(savedPrice);
        }

        [TestMethod]
        public async Task GetPricesByShopIdAsync_InvalidShopId_EmptyListIsReturned()
        {
            var shopId = Guid.NewGuid();

            var savedPrices = await PriceService.GetPricesByShopIdAsync(shopId);

            Assert.IsNotNull(savedPrices);
            Assert.AreEqual(0, savedPrices.Count, "No prices are returned.");
        }

        [TestMethod]
        public async Task GetPricesByProductIdAsync_InvalidProductId_EmptyListIsReturned()
        {
            var productId = Guid.NewGuid();

            var savedPrices = await PriceService.GetPricesByProductIdAsync(productId);

            Assert.IsNotNull(savedPrices);
            Assert.AreEqual(0, savedPrices.Count, "No prices are returned.");
        }

        [TestMethod]
        public async Task AddPriceAsync_PriceWithInvalidUnitsPerPack_ThrowsException()
        {
            var product = new Product()
            {
                Name = "Test Product",
                Description = "Test Product Description",
                UnitOfMeasure = new()
                {
                    Name = "Pieces",
                    Abbreviation = "pcs"
                },
                QuantityPerUnit = 2,
                Variants = "Test Variant"
            };
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                Product = product,
                ShopId = Guid.NewGuid(),
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.AddPriceAsync(price), "Cannot set units per pack if price is not for pack.");
        }

        [TestMethod]
        public async Task AddPriceAsync_PriceWithInvalidPackUnitsPerPack_ThrowsException()
        {
            var product = new Product()
            {
                Name = "Test Product",
                Description = "Test Product Description",
                UnitOfMeasure = new()
                {
                    Name = "Pieces",
                    Abbreviation = "pcs"
                },
                QuantityPerUnit = 2,
                Variants = "Test Variant"
            };
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ShopId = Guid.NewGuid(),
                Product = product,
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.AddPriceAsync(price), "Units per pack should be more than 1 is price per pack.");
        }

        [TestMethod]
        public async Task UpdatePriceAsync_ValidPrice_PriceIsUpdated()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price);

            price.Amount = 12.99M;

            await PriceService.UpdatePriceAsync(price);

            var savedPrice = db.Prices.FirstOrDefault();

            Assert.IsNotNull(savedPrice);
            Assert.AreEqual(price.Amount, savedPrice.Amount, "Amounts are the same.");
        }

        [TestMethod]
        public async Task UpdatePriceAsync_PriceWithInvalidAmount_ThrowsException()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price);

            price.Amount = -1;

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.UpdatePriceAsync(price), "Amount should be greater than 0.");
        }

        [TestMethod]
        public async Task DeletePriceAsync_ValidPriceId_PriceIsDeleted()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id
            };

            await PriceService.AddPriceAsync(price);

            await PriceService.DeletePriceAsync(price.Id);

            var savedPrice = db.Prices.FirstOrDefault();

            Assert.IsNull(savedPrice);
        }

        [TestMethod]
        public async Task DeletePriceAsync_InvalidPriceId_ThrowsException()
        {
            var priceId = Guid.NewGuid();

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.DeletePriceAsync(priceId), "Price not found.");
        }

        [TestMethod]
        public async Task AddPriceAsync_DuplicatePrice_ThrowsException()
        {
            var price = new Price()
            {
                Amount = 10.99M,
                Url = "https://www.example.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id,
            };

            await PriceService.AddPriceAsync(price);

            var duplicatePrice = new Price()
            {
                Amount = 12.99M,
                Url = "https://www.example2.com",
                ProductId = DefaultProduct.Id,
                ShopId = DefaultShop.Id,
            };

            await Assert.ThrowsExceptionAsync<Exception>(async () => await PriceService.AddPriceAsync(duplicatePrice), "Price already exists for the product and shop.");
        }
    }
}