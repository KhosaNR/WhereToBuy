using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using API.Models;
using AutoMapper;
using System.Data.Entity;

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
        public ProductService(DatabaseContext dbContext, IProductSearchService productSearchService, IMapper map)
        {
            this.db = dbContext;
            this.productSearchService = productSearchService;
            this.map = map;
        }
        public async Task AddProductAsync(Product product)
        {
            ValidateProduct(product);

            if (product.Id != Guid.Empty)
            {
                var productExistById = await db.Products.AnyAsync(p => p.Id == product.Id);
                if (productExistById)
                {
                    throw new Exception("Product with Id already exist.");
                }
            }

            var productExistByNameAndVariant = db.Products.Any(p => p.Name == product.Name && p.Variant == product.Variant);
            if (productExistByNameAndVariant)
            {
                throw new Exception("A variant of this product already exist.");
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();
        }

        public async Task<List<Product>> SearchProductAsync(string searchKeywords)
        {
            var products = await productSearchService.SearchProductsAsync(searchKeywords);
            return products;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            ValidateProduct(product);

            var existingProduct = db.Products.FirstOrDefault(p => p.Id == product.Id);

            var products = db.Products.ToList();

            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            map.Map(product,existingProduct);

            await db.SaveChangesAsync();

            return true;
        }

        public void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            if (product.Quantity < 0)
            {
                throw new ArgumentException("Product quantity cannot be negative.");
            }

            if (product.UnitOfMeasure == null)
            {
                throw new ArgumentException("Product unit of measure cannot be empty.");
            }
        }
    }
}
