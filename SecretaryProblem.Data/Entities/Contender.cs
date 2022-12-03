namespace SecretaryProblem.Data;

public class Contender
{
    public Contender()
    {
        
    }
    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
    }
    public int TryId { get; set; }
    
    public int SequenceNumber { get; set; }
    
    public string Name { get; init; }
    
    public int Rating { get; init; }
    
    public override string ToString()
    {
        return $"Name: {Name} Rating:{Rating}";
    }
    
}