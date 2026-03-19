using System.Text.Json.Serialization;
using API.AutoMapper;
using API.EndPoints;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var logDbConnectionString = builder.Configuration.GetConnectionString("LogDatabase");

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    var sinkOptions = new MSSqlServerSinkOptions
    {
        TableName = "SystemLogs",
        AutoCreateSqlTable = true,
    };

    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
            connectionString: logDbConnectionString,
            sinkOptions: sinkOptions);
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperProfile>();
});

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddTransient<IProductSearchService, ProductSearchService>();
builder.Services.AddTransient<IShopService, ShopService>();
builder.Services.AddTransient<IStockListService, StockListService>();
builder.Services.AddTransient<IStockProductService, StockProductService>();

var app = builder.Build();

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

// app.UseAuthorization();
app.MapShopEndPoints();
app.MapProductEndPoints();
app.MapPriceEndPoints();
app.MapStockListEndPoints();
app.MapStockProductEndPoints();

app.Run();