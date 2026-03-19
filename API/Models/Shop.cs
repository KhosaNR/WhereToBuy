namespace API.Models
{
    using API.Models.BaseClasses;

    public class Shop : BaseAuditableEntity
    {
        public string Name { get; set; }

        public Location Location { get; set; }

        public Guid LocationId { get; set; }
    }
}
