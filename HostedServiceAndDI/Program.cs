using HostedServiceAndDI;
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
            .AddScoped<FileWriter>();
    })
    .Build();

host.Run();

