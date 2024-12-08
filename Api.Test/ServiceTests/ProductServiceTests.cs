using API.Models;
using ApiTest;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest
{
    [TestClass]
    public class ProductServiceTests : BaseTestClass
    {
        [TestMethod]
        public void AddProduct_NewValidProduct_NewProductIsAdded()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };

            ProductService.AddProductAsync(product);

            var savedProduct = db.Products.FirstOrDefault();

            Assert.IsNotNull(savedProduct);
            Assert.AreEqual("Koo Baked Beans", savedProduct.Name, "Product names are the same.");
            Assert.IsNotNull(savedProduct.CreatedDate, "Created date is not null.");
            Assert.IsTrue(savedProduct.CreatedDate > DateTime.MinValue, "Created date is not minimum date.");
        }

        [TestMethod]
        public async Task AddProduct_EmptyName_ThrowsException()
        {
            var product = new Product()
            {
                Name = "",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await ProductService.AddProductAsync(product), "Product name cannot be empty.");
        }

        [TestMethod]
        public async Task AddProduct_EmptyUnitOfMeasure_ThrowsException()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = null,
                Quantity = 300,
                Variant = "Original"
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await ProductService.AddProductAsync(product), "Product unit of measure cannot be empty.");
        }

        [TestMethod]
        public async Task AddProduct_NegativeQuantity_ThrowsException()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = -1,
                Variant = "Original"
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await ProductService.AddProductAsync(product), "Product quantity cannot be negative.");
        }

        [TestMethod]
        public async Task AddProduct_Duplicate_ThrowsException()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };
            var duplicateProduct = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };
            ProductService.AddProductAsync(product);

            var products = db.Products.ToList();


            await Assert.ThrowsExceptionAsync<Exception>(async () => await ProductService.AddProductAsync(duplicateProduct), "Duplicate Name and Variant not allowed");
        }

        [TestMethod]
        public async Task SearchProductAsync_ExistingProduct_ReturnsProduct()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };
            await ProductService.AddProductAsync(product);

            var products = await ProductService.SearchProductAsync("beans koo");
            var savedProduct = products.FirstOrDefault();

            Assert.IsNotNull(savedProduct, "Saved product is not null.");
            Assert.IsTrue(savedProduct.Name.Contains("Beans"), "Product name contains first keyword");
            Assert.IsTrue(savedProduct.Name.Contains("Koo"), "Product name contains second keyword");
        }

        [TestMethod]
        public async Task SearchProductAsync_MultipleProducts_ReturnsRankedProducts()
        {
            var productsToAdd = new List<Product>()
            {
                new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            },
                new Product()
            {
                Name = "Koo Peas",
                Description = "Koo Peas 340g",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 340,
                Variant = "Original"
            },
                new Product()
            {
                Name = "Colgate toothpaste",
                Description = "Colgate toothpaste 100ml",
                UnitOfMeasure = new()
                {
                    Name = "Millimeter",
                    Abbreviation = "ml"
                },
                Quantity = 100,
                Variant = "Original"
            }
            };

            foreach (var product in productsToAdd)
            {
                await ProductService.AddProductAsync(product);
            }

            var products = await ProductService.SearchProductAsync("beans koo");
            var kooBeans = products[0];
            var kooPeas = products[1];

            Assert.AreEqual(2, products.Count, "Two products returned by search");
            Assert.IsTrue(kooBeans.Name.Contains("Beans"), "First product name contains first keyword: Beans");
            Assert.IsTrue(kooBeans.Name.Contains("Koo"), "First product name contains second keyword: Koo");
            Assert.IsTrue(!kooPeas.Name.Contains("Beans"), "Second product name does NOT contain first keyword: Beans");
            Assert.IsTrue(kooPeas.Name.Contains("Koo"), "Second product name contains second keyword: Koo");
        }

        [TestMethod]
        public async Task SearchProductAsync_EmptySearchString_ReturnsProduct()
        {
            var product = new Product()
            {
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 300,
                Variant = "Original"
            };
            await ProductService.AddProductAsync(product);

            var products = await ProductService.SearchProductAsync("");
            var savedProduct = products.FirstOrDefault();

            Assert.IsNotNull(savedProduct, "Saved product is not null.");
            Assert.IsTrue(savedProduct.Name.Contains("Beans"), "Product name contains first keyword");
            Assert.IsTrue(savedProduct.Name.Contains("Koo"), "Product name contains second keyword");
        }

        [TestMethod]
        public async Task UpdateProductAsync_ValidProduct_ProductIsSaved()
        {
            var product = new Product()
            {
                Name = "Koo BakedBeans",
                Description = "Koo BakedBeans 300g in tomato sauce",
                UnitOfMeasure = new()
                {
                    Name = "Grams",
                    Abbreviation = "g"
                },
                Quantity = 303,
                Variant = "Origin"
            };
            await ProductService.AddProductAsync(product);

            var changedProduct = new Product()
            {
                Id = product.Id,
                Name = "Koo Baked Beans",
                Description = "Koo Baked Beans 300g in tomato sauce",
                UnitOfMeasure = product.UnitOfMeasure,
                Quantity = 300,
                Variant = "Original"
            };

            await ProductService.UpdateProductAsync(changedProduct);
            var savedProducts = db.Products.ToList();
            var savedProduct = savedProducts.FirstOrDefault();

            Assert.AreEqual(1, savedProducts.Count, "Only one product saved");
            Assert.IsNotNull(savedProduct);
            Assert.AreEqual("Koo Baked Beans", savedProduct.Name, "Product names are the same.");
            Assert.AreEqual("Koo Baked Beans 300g in tomato sauce", savedProduct.Description, "Product descriptions are the same.");
            Assert.AreEqual(300, savedProduct.Quantity, "Product quantities are the same.");
            Assert.AreEqual("Original", savedProduct.Variant, "Product variants are the same.");
            Assert.IsTrue(savedProduct.ModifiedDate > savedProduct.CreatedDate, "Modified date is more recent that created date");
        }
    }
}
