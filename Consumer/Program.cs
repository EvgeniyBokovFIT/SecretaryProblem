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
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
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