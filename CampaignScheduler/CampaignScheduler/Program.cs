using CampaignScheduler;
using CampaignScheduler.Models;
using CampaignScheduler.Services;



IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var loader = new InitialDataLoader(Constants.CAMPAIGNS_PATH, Constants.CUSTOMERS_PATH);
        services.AddSingleton(loader.LoadCampaigns());
        services.AddSingleton(loader.LoadCustomers());

        services.AddSingleton<ISenderService, FileSenderService>();
        services.AddSingleton<ICampaignService, CampaignService>();
        services.AddHostedService<Worker>();
    })
    .Build();



await host.RunAsync();