using CampaignScheduler.Models;

namespace CampaignScheduler.Services
{
    public interface ISenderService
    {
        Task SendCampaignAsync(Campaign campaign, Customer customer);
    }
}
