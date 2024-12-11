using API.Models.BaseClasses;

namespace API.Models
{
    public abstract class User : BaseAuditableEntity
    {
        public String Username { get; set; }
        public List<StockList> StockLists { get; set; }
    }
}
