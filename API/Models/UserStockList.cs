using API.Models.BaseClasses;

namespace API.Models
{
    public class UserStockList : BaseAuditableEntity
    {
        public User User { get; set; }
        public Guid UserId { get; set; }
        public StockList StockList { get; set; }
        public Guid StockListId { get; set; }
        public bool IsActive { get; set; }
    }
}
