namespace CampaignScheduler.Models
{
    public class Campaign
    {
        public string Template { get; set; }
        public Func<Customer, bool> Condition { get; set; }
        public DateTime SendTime { get; set; }
        public int Priority { get; set; }
    }
}
