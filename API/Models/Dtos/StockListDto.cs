namespace API.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class StockListDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public bool IsActive { get; set; }

        public List<StockListProductDto>? Products { get; set; }
    }
}