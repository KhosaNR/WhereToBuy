namespace API.Models
{
    using System.ComponentModel.DataAnnotations;
    using API.Models.BaseClasses;

    public class MeasurementUnit : BaseAuditableEntity
    {
        [Required]
        public string Abbreviation { get; set; }

        public string Name { get; set; }
    }
}
