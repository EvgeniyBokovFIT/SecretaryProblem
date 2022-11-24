namespace HostedServiceAndDI.Entities;

public class Contender
{
    public int TryId { get; set; }
    
    public int SequenceNumber { get; set; }
    
    public string Name { get; init; }
    
    public int Rating { get; init; }
    
    public Contender(string name, int rating)
    {
        Name = name;
        Rating = rating;
    }
}