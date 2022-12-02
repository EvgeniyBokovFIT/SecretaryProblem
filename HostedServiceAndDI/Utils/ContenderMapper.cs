using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Utils;

public static class ContenderMapper
{
    public static Contender MapContenderToDbContender(Contender contender, int contenderNumber, int tryNumber)
    {
        var dbContender = new Contender
        {
            Name = contender.Name, 
            Rating = contender.Rating,
            SequenceNumber = contenderNumber, 
            TryId = tryNumber
        };
        return dbContender;
    }

    public static Contender MapDbContenderToContender(Contender contender)
    {
        return new Contender(contender.Name, contender.Rating);
    }
}