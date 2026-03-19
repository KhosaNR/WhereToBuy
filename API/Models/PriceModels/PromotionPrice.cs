namespace API.Models.PriceModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using API.Models.BaseClasses;

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
