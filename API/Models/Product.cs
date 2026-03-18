using API.Helpers.Enums;
using API.Models.BaseClasses;
using API.Models.PriceModels;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Product : BaseAuditableEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasurementUnit UnitOfMeasure { get; set; }
        public double QuantityPerUnit { get; set; }
        public List<ProductTag>? Tags { get; set; }
        public string Variants { get; set; } = "Original";
        public List<Price>? Prices { get; set; }
        public ProductType ProductType { get; private set; } = ProductType.Unit;
        public uint? UnitsPerPack { get; private set; } = null; // TODO: I don't like having this which is tied to packs here
        public static Product CreateUnitProduct(string name, string description)
        {
            return new Product { Name = name, Description = description, ProductType = ProductType.Unit, UnitsPerPack = null };
        }

        public static Product CreatePackProduct(string name, string description, uint unitsPerPack)
        {
            return new Product { Name = name, Description = description, ProductType = ProductType.Pack, UnitsPerPack = unitsPerPack };
        }
    }
}
