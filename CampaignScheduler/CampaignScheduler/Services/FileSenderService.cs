using CampaignScheduler.Models;

namespace CampaignScheduler.Services
{
    internal class FileSenderService : ISenderService
    {
        private const string SENT_FILE_PATH = @"..\..\SentCampaigns";

        public void SendCampaign(Campaign campaign, Customer customer)
        {
            var now = DateTime.Now.ToString("MM-dd_HH-mm-ss");
            var fileName = $"{SENT_FILE_PATH}\\sends_({now})_{campaign.Template}_Customer_{customer.Id}.txt";

            File.Create(fileName);
        }
    }
}
