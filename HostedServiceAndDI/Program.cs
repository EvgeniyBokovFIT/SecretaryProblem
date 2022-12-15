using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Services;
using HostedServiceAndDI.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretaryProblem.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<Princess>()
            .AddScoped<ContenderRepository>()
            .AddDbContext<EnvironmentContext>()
            .AddScoped<Hall>()
            .AddScoped<Friend>()
            .AddScoped<ContenderGenerator>()
            .AddScoped<FileWriter>()
            .AddScoped<IPrincessBehaviour, MyStrategy>();
    })
    .Build();

host.Run();

