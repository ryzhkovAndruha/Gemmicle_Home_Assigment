using CampaignScheduler.Models;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Globalization;

namespace CampaignScheduler.Services
{
    internal class InitialDataLoader
    {
        private const string CAMPAIGNS_PATH = @"..\..\InitialData\campaigns.csv";
        private const string CUSTOMERS_PATH = @"..\..\InitialData\customers.csv";

        public List<Campaign> LoadCampaigns()
        {
            var campaigns = new List<Campaign>();
            var lines = File.ReadAllLines(CAMPAIGNS_PATH);

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');

                var template = parts[0];
                var condition = parts[1];
                var sendTime = TimeSpan.Parse(parts[2]);
                var priority = int.Parse(parts[3]);

                campaigns.Add(new Campaign
                {
                    Template = template,
                    Condition = ParseConditionAsync(condition),
                    SendTime = DateTime.Today.Add(sendTime),
                    Priority = priority
                });
            }

            return campaigns;
        }

        public List<Customer> LoadCustomers()
        {
            var customers = new List<Customer>();
            var lines = File.ReadAllLines(CUSTOMERS_PATH);

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');

                customers.Add(new Customer
                {
                    Id = int.Parse(parts[0]),
                    Age = int.Parse(parts[1]),
                    Gender = ParseGender(parts[2]),
                    City = parts[3],
                    Deposit = int.Parse(parts[4]),
                    IsNew = parts[5] == "1"
                });
            }

            return customers;
        }

        private Func<Customer, bool> ParseConditionAsync(string condition)
        {
            if (condition.Contains("\'"))
            {
                condition = condition.Replace("\'", "\"");
            }

            var scriptOptions = ScriptOptions.Default.AddReferences(typeof(Customer).Assembly)
                .AddImports("System")
                .AddImports("CampaignScheduler.Models");

            var code = $"new Func<Customer, bool>(customer => customer.{condition})";
            return CSharpScript.EvaluateAsync<Func<Customer, bool>>(code, scriptOptions).Result;
        }

        private Gender ParseGender(string gender)
        {
            if (gender.ToLower().Contains("female"))
            {
                return Gender.Female;
            }
            return Gender.Male;
        }
    }
}
