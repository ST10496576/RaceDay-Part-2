using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("CATEGORIES")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public decimal Distance { get; set; }

        [Required]
        public decimal EntryFee { get; set; }

        [Required]
        public int MaximumParticipants { get; set; }
    }
}