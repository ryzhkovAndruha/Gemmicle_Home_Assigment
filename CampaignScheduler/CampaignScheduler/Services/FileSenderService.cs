using CampaignScheduler.Models;

namespace CampaignScheduler.Services
{
    internal class FileSenderService : ISenderService
    {
        private const string SENT_FILE_PATH = @"..\..\SentCampaigns";
        ILogger<Worker> _logger;

        public FileSenderService(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public async Task SendCampaignAsync(Campaign campaign, Customer customer)
        {
            var now = DateTime.Now.ToString("MM-dd_HH-mm-ss");
            var fileName = $"{SENT_FILE_PATH}\\sends_({now})_{campaign.Template}_Customer_{customer.Id}.txt";
            _logger.Log(LogLevel.Debug, $"{campaign.Template} was sent to client {customer.Id} with priority {campaign.Priority}");

            File.Create(fileName);
            await Task.Delay(TimeSpan.FromMinutes(30));
        }
    }
}
