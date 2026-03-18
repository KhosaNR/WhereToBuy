using API.Models.BaseClasses;
using API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.PriceModels
{
    public class Price : BaseAuditableEntity
    {
        public decimal Amount { get; set; }
        public string Url { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
        public DateTime PriceDate { get; set; } = DateTime.UtcNow;

        public List<PromotionPrice>? PromotionPrices { get; set; }

        public List<PromotionPrice>? ActivePromotionPrices() => PromotionPrices?.Where(pp => pp.IsActive).ToList();

        public PromotionPrice? GetBestActivePromotion()
        {
            var activePromos = ActivePromotionPrices();

            if (activePromos == null || !activePromos.Any())
                return null;

            return activePromos.OrderBy(pp => pp.Amount).First();
        }
    }
}
