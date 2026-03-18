using System.ComponentModel.DataAnnotations;

namespace API.Models.Dtos
{
    public class ShopDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public LocationDto Location { get; set; }
        public Guid LocationId { get; set; }
    }
}
