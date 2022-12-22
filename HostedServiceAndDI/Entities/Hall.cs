using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Services;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Entities;

public class Hall
{
    public Hall()
    {
        Contenders = new Queue<Contender>();
    }
    
    public Queue<Contender> Contenders;

    private readonly ContenderGenerator? _generator;

    public Contender LastViewedContender;

    public List<string>? ContendersNames { get; private set; }

    public Hall(ContenderGenerator generator)
    {
        _generator = generator;
        
        Contenders = new Queue<Contender>(_generator.GenerateContenders());

        ContendersNames = new List<string>();
        
        foreach (var contender in Contenders)
        {
            ContendersNames.Add(contender.Name);
        }
    }

    public virtual int ContendersCount => Contenders.Count;
    
    public virtual Contender GetNextContender()
    {
        if (ContendersCount == 0)
        {
            throw new EmptyHallException("Hall is empty");
        }

        var contender = Contenders.Dequeue();
        LastViewedContender = contender;
        return contender;
    }

    public void FillContenders()
    {
        Contenders = new Queue<Contender>(_generator.GenerateContenders());

        ContendersNames = new List<string>();
        
        foreach (var contender in Contenders)
        {
            ContendersNames.Add(contender.Name);
        }
    }

}