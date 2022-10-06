namespace SecretaryProblem;

public class Friend
{
    public Contender Compare(in Contender first, in Contender second)
    {
        return first.Rating > second.Rating ? first : second;
    }
}