using HostedServiceAndDI;
using HostedServiceAndDI.Entity;
using HostedServiceAndDI.Service;
using HostedServiceAndDI.Strategy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SecretaryProblem.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .AddHostedService<Princess>()
            .AddSingleton<EnvironmentContext>()
            .AddScoped<Hall>()
            .AddScoped<Friend>()
            .AddScoped<ContenderGenerator>()
            .AddScoped<FileWriter>()
            .AddScoped<IPrincessBehaviour, MyStrategy>();
    })
    .Build();

// using (var context = new EnvironmentContext())
// {
//     var dbContender = new DbContender
//     {
//         Name = "Evgeniy", Rating = 100,
//         SequenceNumber = 0, TryId = 0
//     };
//     context.Database.EnsureCreated();
//     context.DbContenders.Add(dbContender);
//     Console.WriteLine("OKOK");
//     context.SaveChanges();
// }


host.Run();

