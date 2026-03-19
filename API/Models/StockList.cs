namespace API.Models
{
    using API.Models.BaseClasses;

    public class StockList : BaseAuditableEntity
    {
        public string Name { get; set; }

        public List<StockListProduct> StockListProducts { get; set; }

        public List<UserStockList> SharedUsers { get; set; }

        public Guid OwnerId { get; set; }

        public User Owner { get; set; }

        public bool IsActive { get; set; }
    }
}
