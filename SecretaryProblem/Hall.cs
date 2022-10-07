using SecretaryProblem.Exceptions;

namespace SecretaryProblem;

public class Hall
{
    private readonly Queue<Contender> _contenders;

    public Hall(Queue<Contender> contenders)
    {
        _contenders = contenders;
    }

    public int ContendersCount => _contenders.Count;
    
    public Contender GetNextContender()
    {
        if (ContendersCount == 0)
        {
            throw new HallException("Hall is empty");
        }

        var contender = _contenders.Dequeue();
        return contender;
    }

}