using CampaignScheduler.Models;
using CampaignScheduler.Services;
using Moq;
using Xunit;

namespace CampaignScheduler_Tests
{
    public class CampaignServiceTests
    {
        const string CAMPAIGNS_PATH = @"..\..\..\..\..\InitialData_ForTests\campaigns.csv";
        const string CUSTOMERS_PATH = @"..\..\..\..\..\InitialData_ForTests\customers.csv";

        private readonly Mock<ISenderService> _mockSenderService;
        private readonly InitialDataLoader _loader;

        public CampaignServiceTests()
        {
            _mockSenderService = new Mock<ISenderService>();

            _loader = new InitialDataLoader(CAMPAIGNS_PATH, CUSTOMERS_PATH);
        }

        [Fact]
        public async Task ScheduleCampaignAsync_Test()
        {
            var campaignService = new CampaignService(_mockSenderService.Object, _loader.LoadCampaigns(), _loader.LoadCustomers());

            await campaignService.ScheduleCampaignAsync();

            _mockSenderService.Verify(
                sender => sender.SendCampaignAsync(
                    It.Is<Campaign>(c => c.Template == "Template A"),
                    It.Is<Customer>(cust => cust.Id == 1)),
                Times.Once);

            _mockSenderService.Verify(
                sender => sender.SendCampaignAsync(
                    It.Is<Campaign>(c => c.Template == "Template A"),
                    It.Is<Customer>(cust => cust.Id == 2)),
                Times.Once);

            _mockSenderService.Verify(
                sender => sender.SendCampaignAsync(
                    It.Is<Campaign>(c => c.Template == "Template B"),
                    It.Is<Customer>(cust => cust.Id == 24)),
                Times.Once);
        }
    }
}
