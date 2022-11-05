namespace HostedServiceAndDI.Entity;

public class Contender
{
    public string Name { get; init; }
    
    public int Rating { get; init; }
    
    public int TryId { get; set; }
    
    public int SequenceNumber { get; set; }

    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
    }
}