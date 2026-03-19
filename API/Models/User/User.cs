namespace API.Models
{
    using API.Models.BaseClasses;

    public class User : BaseAuditableEntity
    {
        public string? Username { get; set; }

        public List<UserStockList> StockLists { get; set; }
    }
}
