namespace API.Models
{
    using API.Models.BaseClasses;

    public class Location : BaseAuditableEntity
    {
        public string Link { get; set; }

        public string Address { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
