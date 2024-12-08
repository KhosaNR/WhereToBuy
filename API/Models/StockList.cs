using API.Models.BaseClasses;

namespace API.Models
{
    public class StockList: BaseAuditableEntity
    {
        public string Name { get; set; }
        public List<StockListProduct> Products { get; set; }
        public List<UserStockList> SharedUsers { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public  bool IsActive { get; set; }
    }
}
