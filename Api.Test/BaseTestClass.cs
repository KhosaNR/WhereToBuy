using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.AutoMapper;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ApiTest
{
    [TestClass]
    public abstract class BaseTestClass : IDisposable
    {
        public BaseTestClass()
        {
            TestInitialize();
        }

        protected DatabaseContext Db => Services.GetService<DatabaseContext>();

        protected ServiceProvider Services { get; private set; }

        protected IProductService ProductService => Services.GetService<IProductService>();

        protected IShopService ShopService => Services.GetService<IShopService>();

        protected IPriceService PriceService => Services.GetService<IPriceService>();

        protected IStockListService StockListService => Services.GetService<IStockListService>();

        [TestInitialize]
        public void TestInitialize()
        {
            RegisterServices();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Db.Database.EnsureDeleted();
            if (Services is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Log.CloseAndFlush();
        }

        public void Dispose()
        {
        }

        private void RegisterServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(logger, dispose: true);
            });

            serviceCollection.AddTransient<IProductService, ProductService>();
            serviceCollection.AddTransient<IPriceService, PriceService>();
            serviceCollection.AddTransient<IProductSearchService, ProductSearchService>();
            serviceCollection.AddTransient<IShopService, ShopService>();
            serviceCollection.AddTransient<IStockListService, StockListService>();

            serviceCollection.AddSingleton<DatabaseContext>(provider =>
            {
                var options = new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .ConfigureWarnings(warnings => warnings.Throw())
                    .Options;

                return new DatabaseContext(options);
            });

            Services = serviceCollection.BuildServiceProvider();
        }
    }
}