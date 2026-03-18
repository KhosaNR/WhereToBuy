using API.Models.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.PriceModels
{
    public class PromotionPrice : BaseAuditableEntity
    {
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }

        [NotMapped]
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        public decimal Amount { get; set; }
        public uint? Quantity { get; set; }

        [Required]
        public Guid PriceId { get; set; }

        public Price Price { get; set; }
    }
}
