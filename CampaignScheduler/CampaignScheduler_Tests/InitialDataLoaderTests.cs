using CampaignScheduler.Models;
using CampaignScheduler.Services;
using Xunit;

namespace CampaignScheduler_Tests
{
    public class InitialDataLoaderTests
    {
        const string CAMPAIGNS_PATH = @"..\..\..\..\..\InitialData_ForTests\campaigns.csv";
        const string CUSTOMERS_PATH = @"..\..\..\..\..\InitialData_ForTests\customers.csv";

        private readonly InitialDataLoader _loader;

        public InitialDataLoaderTests()
        {
            _loader = new InitialDataLoader(CAMPAIGNS_PATH, CUSTOMERS_PATH);
        }

        [Fact]
        public void LoadCampaigns_ShouldLoadCampaignsCorrectly()
        {
            var campaigns = _loader.LoadCampaigns();

            Assert.NotNull(campaigns);
            Assert.Equal(5, campaigns.Count);
            Assert.Equal("Template A", campaigns[0].Template);
            Assert.Equal("Template B", campaigns[1].Template);
            Assert.Equal("Template C", campaigns[2].Template);
            Assert.Equal("Template A", campaigns[3].Template);
            Assert.Equal("Template C", campaigns[4].Template);
        }

        [Fact]
        public void LoadCustomers_ShouldLoadCustomersCorrectly()
        {
            var customers = _loader.LoadCustomers();

            Assert.NotNull(customers);
            Assert.Equal(40, customers.Count);
            Assert.Equal(53, customers[0].Age);
            Assert.Equal(Gender.Male, customers[0].Gender);
            Assert.Equal("London", customers[0].City);
            Assert.Equal(104, customers[0].Deposit);
            Assert.False(customers[0].IsNew);

            Assert.Equal(44, customers[1].Age);
            Assert.Equal(Gender.Female, customers[1].Gender);
            Assert.Equal("New York", customers[1].City);
            Assert.Equal(209, customers[1].Deposit);
            Assert.True(customers[1].IsNew);
        }

        [Fact]
        public void ParseConditionAsync_ShouldParseConditionCorrectly()
        {
            var campaigns = _loader.LoadCampaigns();
            var customers = _loader.LoadCustomers();

            var filteredCustomers = customers.Where(c => campaigns.Any(ca => ca.Condition(c))).ToList();

            Assert.Contains(filteredCustomers, c => c.Age > 45);
            Assert.Contains(filteredCustomers, c => c.City == "New York");
            Assert.Contains(filteredCustomers, c => c.Deposit > 100);
        }
    }
}
