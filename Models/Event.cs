using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("EVENT")]
    public class Event
    {
        [Key]
        [Column("EVENTID")]
        public int EventID { get; set; }

        [Required]
        public int OrganiserID { get; set; }

        [Required]
        public string EventName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Province { get; set; } = string.Empty;

        [Required]
        public DateTime RaceDate { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }
    }
}