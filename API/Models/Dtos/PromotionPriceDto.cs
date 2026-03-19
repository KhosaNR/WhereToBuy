namespace API.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class PromotionPriceDto
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Amount { get; set; }

        public uint? Quantity { get; set; }

        public Guid PriceId { get; set; }
    }
}