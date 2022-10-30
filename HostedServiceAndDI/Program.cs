using HostedServiceAndDI;
using HostedServiceAndDI.entity;
using HostedServiceAndDI.service;
using HostedServiceAndDI.strategy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<Princess>()
            .AddScoped<Hall>()
            .AddScoped<Friend>()
            .AddScoped<ContenderGenerator>()
            .AddScoped<FileWriter>()
            .AddScoped<IPrincessBehaviour, MyStrategy>();
    })
    .Build();

host.Run();

