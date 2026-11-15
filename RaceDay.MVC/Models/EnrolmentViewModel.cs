namespace RaceDay.MVC.Models
{
    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public int ParticipantID { get; set; }

        public int CategoryID { get; set; }

        public DateTime RegistrationDaate { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public decimal EntryFee { get; set; }

        public string EventName { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;

        public DateTime RaceDate { get; set; }
    }
}