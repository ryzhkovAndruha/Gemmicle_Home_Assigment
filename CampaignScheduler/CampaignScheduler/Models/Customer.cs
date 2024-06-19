namespace CampaignScheduler.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Customer
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public int Deposit { get; set; }
        public bool IsNew { get; set; }

        public bool HasReceivedCompaignToday { get; set; }
    }
}
