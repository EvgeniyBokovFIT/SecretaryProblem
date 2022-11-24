using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Utils;

public static class ContenderMapper
{
    public static DbContender MapContenderToDbContender(Contender contender, int contenderNumber, int tryNumber)
    {
        var dbContender = new DbContender
        {
            Name = contender.Name, Rating = contender.Rating,
            SequenceNumber = contenderNumber, TryId = tryNumber
        };
        return dbContender;
    }
}