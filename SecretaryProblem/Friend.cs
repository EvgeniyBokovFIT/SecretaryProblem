namespace SecretaryProblem;

public class Friend
{
    public Contender Compare(Contender first, Contender second)
    {
        return first.Rating > second.Rating ? first : second;
    }
}