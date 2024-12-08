using API.Models.BaseClasses;

namespace API.Models
{
    public class Shop : BaseAuditableEntity
    {
        public string Name { get; set; }
        public Location Location { get; set; }
        public  Guid LocationId { get; set; }

    }
}
