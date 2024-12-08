using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.PriceModels
{
    public class PromotionPrice : Price
    {
        public PromotionPrice()
        {
            IsPromotion = true;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        public override bool IsActive => DateTime.UtcNow <= EndDate;
        public bool IsBulk { get; set; }
        public uint? PerBulk { get; set; }
        public bool IsPromotion { get; private set; }
    }
}
