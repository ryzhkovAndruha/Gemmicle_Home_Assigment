using CampaignScheduler;
using CampaignScheduler.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var loader = new InitialDataLoader();
        services.AddSingleton(loader.LoadCampaigns());
        services.AddSingleton(loader.LoadCustomers());

        services.AddSingleton<ISenderService, FileSenderService>();
        services.AddSingleton<ICampaignService, CampaignService>();
        services.AddHostedService<Worker>();
    })
    .Build();



await host.RunAsync();