namespace RaceDay.MVC.Models
{
    public class EnrolmentResultViewModel
    {
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public string ParticipantName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        public decimal? AveragePace { get; set; }

        public int EventID { get; set; }    
    }
}