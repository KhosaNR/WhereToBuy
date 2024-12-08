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
        public double Quantity { get; set; }
        public List<ProductTag> Tags { get; set; }
        public string Variant { get; set; }
        public List<Price> Prices { get; set; }
    }
}
