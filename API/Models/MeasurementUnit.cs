using API.Models.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class MeasurementUnit : BaseAuditableEntity
    {
        [Required]
        public string Abbreviation { get; set; }
        public string Name { get; set; }
    }
}
