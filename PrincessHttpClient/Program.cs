using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrincessHttpClient;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<PrincessClient>()
            .AddSingleton<StrategyClient>();
    })
    .Build();

host.Run();
