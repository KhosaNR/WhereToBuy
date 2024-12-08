using API.AutoMapper;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest
{
    [TestClass]
    public abstract class BaseTestClass
    {
        protected DatabaseContext db => Services.GetService<DatabaseContext>();
        protected ServiceProvider Services { get; private set; }
        protected IProductService ProductService => Services.GetService<IProductService>();
        protected IShopService ShopService => Services.GetService<IShopService>();
        protected IPriceService PriceService => Services.GetService<IPriceService>();

        [TestInitialize]
        public void TestInitialize()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAutoMapper(typeof(AutoMapperProfile));
            serviceCollection.AddTransient<IProductService, ProductService>();
            serviceCollection.AddTransient<IPriceService, PriceService>();
            serviceCollection.AddTransient<IProductSearchService, ProductSearchService>();
            serviceCollection.AddTransient<IPricingService, PricingService>();
            serviceCollection.AddTransient<IShopService, ShopService>();
            serviceCollection.AddSingleton<DatabaseContext>(provider =>
            {
                var options = new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;
                return new DatabaseContext(options);
            });
            Services = serviceCollection.BuildServiceProvider(); ;
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Database.EnsureDeleted();
            if (Services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

    }
}
