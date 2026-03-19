namespace API.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;
    using API.Helpers.Enums;

    public class ProductDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public double QuantityPerUnit { get; set; }

        public string Variants { get; set; }

        public ProductType ProductType { get; set; }

        public uint? UnitsPerPack { get; set; }
    }
}