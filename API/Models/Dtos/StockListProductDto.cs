namespace API.Models.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class StockListProductDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public uint Quantity { get; set; }

        public Guid StockListId { get; set; }
    }
}