using API.Models.BaseClasses;

namespace API.Models
{
    public class User : BaseAuditableEntity
    {
        public String? Username { get; set; }
        public List<UserStockList> StockLists { get; set; }
    }
}
