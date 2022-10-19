using SecretaryProblem.Exceptions;

namespace SecretaryProblem;

public class Friend
{
    public List<Contender> ViewedContenders { get; set; }

    public Friend()
    {
        ViewedContenders = new List<Contender>();
    }

    public Contender Compare(in Contender first, in Contender second)
    {
        if (!ViewedContenders.Contains(first) || !ViewedContenders.Contains(second))
        {
            throw new UnviewedContenderComparingException("Contender was not viewed");
        }
        return first.Rating > second.Rating ? first : second;
        
    }
}