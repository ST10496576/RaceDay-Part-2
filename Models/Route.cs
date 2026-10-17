using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("ROUTE")]
    public class Route
    {
        [Key]
        public int RouteID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public string RouteName { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public string? MapURL { get; set; }

        public decimal? ElevationGain { get; set; }
    }
}