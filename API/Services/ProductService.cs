using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using API.Models;
using AutoMapper;
using System.Data.Entity;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<List<Product>> SearchProductAsync(string searchKeywords);
        Task<bool> UpdateProductAsync(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly DatabaseContext db;
        private readonly IProductSearchService productSearchService;
        private readonly IMapper map;
        private readonly ILogger<ProductService> logger;

        public ProductService(DatabaseContext dbContext, IProductSearchService productSearchService, IMapper map, ILogger<ProductService> logger)
        {
            this.db = dbContext;
            this.productSearchService = productSearchService;
            this.map = map;
            this.logger = logger;
        }

        public async Task AddProductAsync(Product product)
        {
            ValidateProduct(product);

            if (product.Id != Guid.Empty)
            {
                var productExistById = await db.Products.AnyAsync(p => p.Id == product.Id);
                if (productExistById)
                {
                    logger.LogWarning("AddProductAsync failed: Product with ID {ProductId} already exists.", product.Id);
                    throw new Exception("Product with Id already exist.");
                }
            }

            var productExistByNameAndVariant = db.Products.Any(p => p.Name == product.Name && p.Variants == product.Variants);
            if (productExistByNameAndVariant)
            {
                logger.LogWarning("AddProductAsync failed: Product variant '{ProductName}' - '{Variants}' already exists.", product.Name, product.Variants);
                throw new Exception("A variant of this product already exist.");
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            logger.LogInformation("Successfully added product '{ProductName}' with ID {ProductId}.", product.Name, product.Id);
        }

        public async Task<List<Product>> SearchProductAsync(string searchKeywords)
        {
            logger.LogInformation("Delegating search for keywords: {SearchKeywords}", searchKeywords);
            var products = await productSearchService.SearchProductsAsync(searchKeywords);
            return products;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            ValidateProduct(product);

            var existingProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                logger.LogWarning("UpdateProductAsync failed: Product {ProductId} not found.", product.Id);
                throw new Exception("Product not found.");
            }

            map.Map(product, existingProduct);

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully updated product {ProductId}.", product.Id);

            return true;
        }

        public void ValidateProduct(Product product)
        {
            if (product == null)
            {
                logger.LogWarning("Validation failed: Product is null.");
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                logger.LogWarning("Validation failed: Product name is empty.");
                throw new ArgumentException("Product name cannot be empty.");
            }

            if (product.QuantityPerUnit < 0)
            {
                logger.LogWarning("Validation failed: Product quantity {Quantity} is negative.", product.QuantityPerUnit);
                throw new ArgumentException("Product quantity cannot be negative.");
            }

            if (product.UnitOfMeasure == null)
            {
                logger.LogWarning("Validation failed: Product Unit of Measure is null.");
                throw new ArgumentException("Product unit of measure cannot be empty.");
            }
        }
    }
}