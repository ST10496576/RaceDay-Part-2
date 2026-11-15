namespace RaceDay.MVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public DateTime RaceDate { get; set; }
        public DateTime RegistrationDeadline { get; set; }
    }
}