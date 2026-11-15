namespace RaceDay.MVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public decimal EntryFee { get; set; }

        public int MaximumParticipants { get; set; }
    }
}