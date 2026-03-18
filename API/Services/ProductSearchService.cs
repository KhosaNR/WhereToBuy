using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.Extensions.Logging;

public interface IProductSearchService
{
    Task<List<Product>> SearchProductsAsync(string searchString);
}

public class ProductSearchService : IProductSearchService
{
    private readonly DatabaseContext db;
    private readonly ILogger<ProductSearchService> logger;

    public ProductSearchService(DatabaseContext context, ILogger<ProductSearchService> logger)
    {
        db = context;
        this.logger = logger;
    }

    public async Task<List<Product>> SearchProductsAsync(string searchString)
    {
        logger.LogInformation("Searching products with keyword(s): {SearchString}", searchString);

        if (searchString.Trim() == string.Empty)
        {
            logger.LogInformation("Search string is empty. Returning all products.");
            return db.Products.ToList();
        }
        var keywords = searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var predicate = BuildPredicate(keywords);

        var products = db.Set<Product>()
            .Where(predicate)
            .ToList();

        var query = products
            .Select(p => new
            {
                Product = p,
                Rank = keywords.Sum(k =>
                    (p.Name.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.Description.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.UnitOfMeasure.Name.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.QuantityPerUnit.ToString().Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.Variants.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0))
            })
            .OrderByDescending(x => x.Rank)
            .Select(x => x.Product)
            .ToList();

        logger.LogInformation("Search completed. Found {ProductCount} matching products.", query.Count);

        return query;
    }

    private Expression<Func<Product, bool>> BuildPredicate(string[] keywords)
    {
        var predicate = PredicateBuilder.New<Product>(false);

        foreach (var keyword in keywords)
        {
            var temp = keyword;
            predicate = predicate.Or(p => p.Name.Contains(temp, StringComparison.InvariantCultureIgnoreCase) ||
                                          p.Description.Contains(temp, StringComparison.InvariantCultureIgnoreCase) ||
                                          p.UnitOfMeasure.Name.Contains(temp, StringComparison.InvariantCultureIgnoreCase) ||
                                          p.QuantityPerUnit.ToString().Contains(temp, StringComparison.InvariantCultureIgnoreCase) ||
                                          p.Variants.Contains(temp, StringComparison.InvariantCultureIgnoreCase));
        }

        return predicate;
    }
}