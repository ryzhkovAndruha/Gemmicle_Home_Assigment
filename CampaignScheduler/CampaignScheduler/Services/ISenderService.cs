using CampaignScheduler.Models;

namespace CampaignScheduler.Services
{
    internal interface ISenderService
    {
        Task SendCampaignAsync(Campaign campaign, Customer customer);
    }
}
