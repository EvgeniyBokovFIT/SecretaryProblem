using SecretaryProblem.exception;

namespace SecretaryProblem;

public class Hall
{
    private List<Contender> Contenders { get; init; }

    public Hall(List<Contender> contenders)
    {
        Contenders = contenders;
    }

    public int ContendersCount => Contenders.Count;
    
    public Contender GetNextContender()
    {
        if (Contenders.Count == 0)
        {
            throw new HallException("Hall is empty");
        }
        var contender = Contenders[0];
        Contenders.RemoveAt(0);
        return contender;
    }

}