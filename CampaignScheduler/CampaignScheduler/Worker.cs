using CampaignScheduler.Services;

namespace CampaignScheduler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICampaignService _campaignService;

        public Worker(ILogger<Worker> logger, ICampaignService campaignService)
        {
            _logger = logger;
            _campaignService = campaignService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _campaignService.ScheduleCampaign();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}