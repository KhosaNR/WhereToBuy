using API.Models.BaseClasses;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.PriceModels
{
    public class Price : BaseAuditableEntity
    {
        [NotMapped]
        public virtual bool IsActive { get; set; } = true;
        public double Amount { get; set; }
        public string Url { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public Shop Shop { get; set; }
        public Guid ShopId { get; set; }
        public DateTime PriceDate { get; set; } = DateTime.Now;
        public bool IsPack { get; set; } = false;
        public uint? UnitsPerPack { get; set; }
    }
}
