using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.service;

namespace HostedServiceAndDI.entity;

public class Hall
{
    private readonly Queue<Contender> _contenders;

    public List<string> ContendersNames { get; }

    public Hall()
    {
        _contenders = new Queue<Contender>();
        ContendersNames = new List<string>();
    }
    public Hall(ContenderGenerator generator)
    {
        _contenders = new Queue<Contender>(generator.GenerateContenders());

        ContendersNames = new List<string>();
        
        foreach (var contender in _contenders)
        {
            ContendersNames.Add(contender.Name);
        }
    }

    public virtual int ContendersCount => _contenders.Count;
    
    public virtual Contender GetNextContender()
    {
        if (ContendersCount == 0)
        {
            throw new EmptyHallException("Hall is empty");
        }

        var contender = _contenders.Dequeue();
        return contender;
    }

}