using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqKit; // Ensure you have LinqKit installed via NuGet

public interface IProductSearchService
{
    Task<List<Product>> SearchProductsAsync(string searchString);
}

public class ProductSearchService : IProductSearchService
{
    private readonly DatabaseContext db;

    public ProductSearchService(DatabaseContext context)
    {
        db = context;
    }

    public async Task<List<Product>> SearchProductsAsync(string searchString)
    {
        if (searchString.Trim() == string.Empty)
        {
            return db.Products.ToList();
        }
        var keywords = searchString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var predicate = BuildPredicate(keywords);

        var products =  db.Set<Product>()
            .Where(predicate)
            .ToList();// Fetch data from the database first

        var query = products
            .Select(p => new
            {
                Product = p,
                Rank = keywords.Sum(k =>
                    (p.Name.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.Description.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.UnitOfMeasure.Name.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.Quantity.ToString().Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0) +
                    (p.Variant.Contains(k, StringComparison.InvariantCultureIgnoreCase) ? 1 : 0))
            })
            .OrderByDescending(x => x.Rank)
            .Select(x => x.Product)
            .ToList();

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
                                          p.Quantity.ToString().Contains(temp, StringComparison.InvariantCultureIgnoreCase) ||
                                          p.Variant.Contains(temp, StringComparison.InvariantCultureIgnoreCase));
        }

        return predicate;
    }
}
