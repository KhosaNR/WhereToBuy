namespace API.Services
{
    using API.Models;
    using API.Models.Dtos;
    using global::AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public interface IProductService
    {
        Task<CursorPagedResult<Product>> GetAllProductsAsync(int pageSize, DateTime? cursor);

        Task<Product?> GetProductAsync(Guid id);

        Task AddProductAsync(Product product);

        Task<List<Product>> SearchProductAsync(string searchKeywords);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(Guid id);
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

        public async Task<CursorPagedResult<Product>> GetAllProductsAsync(int pageSize, DateTime? cursor)
        {
            var query = db.Products.AsNoTracking().OrderByDescending(p => p.CreatedDate).AsQueryable();

            if (cursor.HasValue)
            {
                query = query.Where(p => p.CreatedDate < cursor.Value);
            }

            var items = await query.Take(pageSize + 1).ToListAsync();
            var hasNextPage = items.Count > pageSize;
            var resultItems = hasNextPage ? items.Take(pageSize).ToList() : items;
            var nextCursor = hasNextPage ? resultItems.Last().CreatedDate : (DateTime?)null;

            return new CursorPagedResult<Product>
            {
                Data = resultItems,
                NextCursor = nextCursor,
            };
        }

        public async Task<Product?> GetProductAsync(Guid id)
        {
            return await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
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

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await db.Products.FindAsync(id);

            if (product == null)
            {
                logger.LogWarning("DeleteProductAsync failed: Product {ProductId} not found.", id);
                throw new Exception("Product not found.");
            }

            product.IsDeleted = true;
            product.DeletedDate = DateTime.UtcNow;

            await db.SaveChangesAsync();

            logger.LogInformation("Successfully soft-deleted product {ProductId}.", id);

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
        }
    }
}