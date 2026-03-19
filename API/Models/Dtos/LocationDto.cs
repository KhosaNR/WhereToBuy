namespace API.Models.Dtos
{
    public class LocationDto
    {
        public Guid Id { get; set; }

        public string Link { get; set; }

        public string Address { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
