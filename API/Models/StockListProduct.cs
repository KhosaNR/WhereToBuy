using API.Models.BaseClasses;
using API.Models;

namespace API.Models
{
    public class StockListProduct : BaseAuditableEntity
    {
        public uint Quantity { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
    }
}
