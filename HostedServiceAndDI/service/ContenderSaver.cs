using HostedServiceAndDI.Entity;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.service;

public static class ContenderSaver
{
    public static void SaveContenders(EnvironmentContext context, IEnumerable<Contender> contenders, int tryNumber)
    {
        context.Database.EnsureCreated();
        int contenderNumber = 1;
        foreach (var contender in contenders)
        {
            var dbContender = new DbContender
            {
                Name = contender.Name, Rating = contender.Rating,
                SequenceNumber = contenderNumber, TryId = tryNumber
            };
            context.DbContenders.Add(dbContender);
            contenderNumber++;
        }

        context.SaveChanges();
    }
}