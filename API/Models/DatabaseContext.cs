using System;
using System.Collections.Generic;
using API.Models;
using API.Models.BaseClasses;
using API.Models.PriceModels;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Models;

public partial class DatabaseContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
    public DbSet<ProductTag> Tag { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<PromotionPrice> PromotionPrices { get; set; }
    public DbSet<StockList> StockLists { get; set; }
    //public DbSet<UserStockList> UserStockLists { get; set; }
    public DbSet<StockListProduct> StoocklistProducts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Prices)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId);

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.Variant })
            .IsUnique();

        modelBuilder.Entity<MeasurementUnit>()
            .HasIndex(u => u.Name)
            .IsUnique();

        modelBuilder.Entity<MeasurementUnit>()
            .HasIndex(u => u.Abbreviation)
            .IsUnique();

        modelBuilder.Entity<PromotionPrice>()
            .HasBaseType<Price>();

        modelBuilder.Entity<Price>()
            .HasDiscriminator<string>("PriceType")
            .HasValue<Price>("Normal")
            .HasValue<PromotionPrice>("Promotion");

        //modelBuilder.Entity<UserStockList>()
        //    .HasBaseType<BaseAuditableEntity>();

        modelBuilder.Entity<Price>()
            .HasOne(p => p.Shop)
            .WithMany()
            .HasForeignKey(p => p.ShopId);

        modelBuilder.Entity<BaseAuditableEntity>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<StockList>()
            .HasOne(s => s.Creator)
            .WithMany()
            .HasForeignKey(s => s.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockList>()
            .HasOne(s => s.Creator)
            .WithMany()
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockListProduct>()
            .HasOne(sl => sl.Product)
            .WithMany()
            .HasForeignKey(sl => sl.ProductId);

        modelBuilder.Entity<UserStockList>()
            .HasIndex(us => new { us.UserId, us.StockListId })
            .IsUnique();

        modelBuilder.Entity<UserStockList>()
            .HasOne(us => us.StockList)
            .WithMany(s => s.SharedUsers)
            .HasForeignKey(us => us.StockListId);

        //modelBuilder.Entity<UserStockList>()
        //    .HasOne(us => us.User)
        //    .WithMany(u => u.StockLists)
        //    .HasForeignKey(us => us.UserId);

        OnModelCreatingPartial(modelBuilder);

    }

    public override async Task<int>  SaveChangesAsync(CancellationToken cancellationToken = default)
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
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
