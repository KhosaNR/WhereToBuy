namespace API.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class PriceDto
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public string Url { get; set; }

        public Guid ProductId { get; set; }

        public Guid ShopId { get; set; }

        public DateTime PriceDate { get; set; }

        public List<PromotionPriceDto>? PromotionPrices { get; set; }
    }
}