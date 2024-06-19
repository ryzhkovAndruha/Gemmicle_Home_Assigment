using CampaignScheduler.Models;

namespace CampaignScheduler.Services
{
    public class CampaignService: ICampaignService
    {
        private readonly ISenderService _senderService;

        private readonly InitialDataLoader _loader;

        private List<Campaign> _campaigns;
        private List<Customer> _customers;

        private readonly Dictionary<Customer, Campaign> _customersCampaigns;

        public CampaignService(ISenderService senderService, List<Campaign> campaigns, List<Customer> customers)
        {
            _loader = new InitialDataLoader(Constants.CAMPAIGNS_PATH, Constants.CUSTOMERS_PATH);
            _senderService = senderService;
            _campaigns = campaigns.OrderBy(c => c.Priority).ThenBy(c => c.SendTime).ToList();
            _customers = customers;

            _customersCampaigns = new Dictionary<Customer, Campaign>();

            FillCustomerCampaignsByPriority();
            MidnightTimerInit();
        }

        public async Task ScheduleCampaignAsync()
        {
            var now = DateTime.Now;
            var sendingTasks = new List<Task>();

            foreach (var customerCampaign in _customersCampaigns.Where(cc => now.TimeOfDay >= cc.Value.SendTime.TimeOfDay).ToList())
            {
                var sendTask = _senderService.SendCampaignAsync(customerCampaign.Value, customerCampaign.Key);
                Task.Run(async () => await sendTask);

                _customersCampaigns.Remove(customerCampaign.Key);
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

        private void RecreateDataOnNewDay(object state)
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
