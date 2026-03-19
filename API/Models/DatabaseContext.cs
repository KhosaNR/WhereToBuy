using System.Reflection;
using API.Models.BaseClasses;
using API.Models.PriceModels;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
    : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }

    public DbSet<ProductTag> Tag { get; set; }

    public DbSet<Price> Prices { get; set; }

    public DbSet<PromotionPrice> PromotionPrices { get; set; }

    public DbSet<StockList> StockLists { get; set; }

    public DbSet<StockListProduct> StockListProducts { get; set; }

    public DbSet<UserStockList> UserStockLists { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Shop> Shops { get; set; }

    public DbSet<Location> Locations { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.ModifiedDate = DateTime.UtcNow;
                entry.Entity.IsDeleted = true;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entityTypes = typeof(BaseAuditableEntity).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseAuditableEntity)));

        var setQueryFilterMethod = typeof(DatabaseContext).GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType).ToTable(entityType.Name);
        }

        // Apply query filters only to the root entities
        foreach (var entityType in entityTypes)
        {
            if (modelBuilder.Entity(entityType).Metadata.BaseType == null)
            {
                setQueryFilterMethod?.MakeGenericMethod(entityType).Invoke(this, new object[] { modelBuilder });
            }
        }

        modelBuilder.Entity<Price>()
            .HasMany(p => p.PromotionPrices)
            .WithOne(pp => pp.Price)
            .HasForeignKey(pp => pp.PriceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Prices)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MeasurementUnit>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<MeasurementUnit>()
            .HasIndex(u => u.Abbreviation)
            .IsUnique();

        modelBuilder.Entity<Price>()
            .HasOne(p => p.Shop)
            .WithMany()
            .HasForeignKey(p => p.ShopId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StockList>()
            .HasOne(s => s.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StockListProduct>()
            .HasOne(sl => sl.Product)
            .WithMany()
            .HasForeignKey(sl => sl.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<StockListProduct>()
            .HasOne(sl => sl.StockList)
            .WithMany(s => s.StockListProducts)
            .HasForeignKey(sl => sl.StockListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserStockList>()
            .HasIndex(us => new { us.UserId, us.StockListId })
            .IsUnique();

        modelBuilder.Entity<UserStockList>()
            .HasOne(us => us.StockList)
            .WithMany(s => s.SharedUsers)
            .HasForeignKey(us => us.StockListId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserStockList>()
            .HasOne(us => us.User)
            .WithMany(u => u.StockLists)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Shop>()
            .HasOne(s => s.Location)
            .WithOne()
            .HasForeignKey<Shop>(s => s.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

        // Overwrite the generic filter for PromotionPrice to include both the soft-delete check and the EndDate condition.
        modelBuilder.Entity<PromotionPrice>()
            .HasQueryFilter(pp => !pp.IsDeleted && pp.EndDate >= DateTime.UtcNow);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private void SetGlobalQueryFilter<T>(ModelBuilder builder)
    where T : BaseAuditableEntity
    {
        builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }
}