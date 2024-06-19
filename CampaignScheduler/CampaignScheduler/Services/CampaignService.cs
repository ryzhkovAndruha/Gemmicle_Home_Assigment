using CampaignScheduler.Models;
using Microsoft.Extensions.Logging;

namespace CampaignScheduler.Services
{
    internal class CampaignService: ICampaignService
    {
        private readonly ISenderService _senderService;

        private readonly InitialDataLoader _loader;
        private Timer _newDayTimer;

        private List<Campaign> _campaigns;
        private List<Customer> _customers;

        private readonly Dictionary<Customer, Campaign> _customersCampaigns;

        public CampaignService(ISenderService senderService, List<Campaign> campaigns, List<Customer> customers)
        {
            _loader = new InitialDataLoader();
            _senderService = senderService;
            _campaigns = campaigns.OrderBy(c => c.Priority).ThenBy(c => c.SendTime).ToList();
            _customers = customers;

            _customersCampaigns = new Dictionary<Customer, Campaign>();

            FillCustomerCampaignsByPriority();
            MidnightTimerInit();
        }

        public void ScheduleCampaign()
        {
            var now = DateTime.Now;

            foreach (var customerCampaign in _customersCampaigns)
            {
                if (now.TimeOfDay >= customerCampaign.Value.SendTime.TimeOfDay)
                {
                    _senderService.SendCampaign(customerCampaign.Value, customerCampaign.Key);
                    _customersCampaigns.Remove(customerCampaign.Key);
                }
            }
        }

        private void FillCustomerCampaignsByPriority()
        {
            foreach (var campaign in _campaigns)
            {
                var targetCustomers = _customers.Where(campaign.Condition);
                foreach (var customer in targetCustomers)
                {
                    if (!customer.HasReceivedCompaignToday)
                    {
                        _customersCampaigns.Add(customer, campaign);
                        customer.HasReceivedCompaignToday = true;
                    }
                }
            }
        }

        private void MidnightTimerInit()
        {
            DateTime now = DateTime.Now;
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0).AddDays(1);
            var timeToGo = (targetTime - now).TotalMilliseconds;

            var timer = new Timer(RecreateDataOnNewDay, null, (int)timeToGo, Timeout.Infinite);
        }

        private void RecreateDataOnNewDay(object state = default)
        {
            ReloadCustomers();
            ReloadCampaigns();
            FillCustomerCampaignsByPriority();

            MidnightTimerInit();
        }

        private void ReloadCampaigns()
        {
            _campaigns.Clear();
            _campaigns = _loader.LoadCampaigns();
        }

        private void ReloadCustomers()
        {
            _customers.Clear();
            _customers = _loader.LoadCustomers();
        }
    }
}
