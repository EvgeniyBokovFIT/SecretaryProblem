using HostedServiceAndDI.Exceptions;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Entities;

public class Friend
{
    public List<Contender> ViewedContenders { get; set; }

    public Friend()
    {
        ViewedContenders = new List<Contender>();
    }

    public Contender Compare(in Contender first, in Contender second)
    {
        if (first is null || second is null || !ViewedContenders.Contains(first) || !ViewedContenders.Contains(second))
        {
            throw new UnviewedContenderException("Contender was not viewed");
        }
        return first.Rating > second.Rating ? first : second;
        
    }
}