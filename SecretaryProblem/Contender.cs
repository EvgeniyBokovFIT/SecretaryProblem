namespace SecretaryProblem;

public class Contender
{
    public string Name { get; init; }
    
    public int Rating { get; init; }

    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
    }
}