using Consumer;
using MassTransit;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://23.99.134.228:5672/7re-qnd-5pu-hld",h =>
                {
                    h.Username("7re-qnd-5pu-hld");
                    h.Password("7re-qnd-5pu-hld");
                });
                cfg.ReceiveEndpoint("contenders", e => 
                    { e.Consumer<PrincessClient>(); }
                );
            });
        });
        //services.AddMassTransitHostedService();
    })
    .Build()
    .RunAsync();