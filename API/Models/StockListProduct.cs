namespace API.Models
{
    using API.Models;
    using API.Models.BaseClasses;

    public class StockListProduct : BaseAuditableEntity
    {
        public uint Quantity { get; set; }

        public Product? Product { get; set; }

        public Guid ProductId { get; set; }

        public Guid StockListId { get; set; }

        public StockList? StockList { get; set; }
    }
}
