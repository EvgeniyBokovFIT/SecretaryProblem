using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Service;

namespace HostedServiceAndDI.Entity;

public class Hall
{
    private Queue<Contender> _contenders;

    private readonly ContenderGenerator _generator;

    public List<string> ContendersNames { get; private set; }

    public Hall(ContenderGenerator generator)
    {
        _generator = generator;
        
        _contenders = new Queue<Contender>(_generator.GenerateContenders());

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

    public void FillContenders()
    {
        _contenders = new Queue<Contender>(_generator.GenerateContenders());

        ContendersNames = new List<string>();
        
        foreach (var contender in _contenders)
        {
            ContendersNames.Add(contender.Name);
        }
    }

}