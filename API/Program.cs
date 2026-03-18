using API.AutoMapper;
using API.Services;
using Microsoft.EntityFrameworkCore;
using API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddTransient<IProductSearchService, ProductSearchService>();
builder.Services.AddTransient<IPricingService, PricingService>();
builder.Services.AddTransient<IShopService, ShopService>();
builder.Services.AddTransient<IStockListService, StockListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DatabaseContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
