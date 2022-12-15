using SecretaryProblem.Data;

namespace HostedServiceAndDI.Repositories;

public class ContenderRepository
{
    private readonly EnvironmentContext _context;

    public ContenderRepository(EnvironmentContext context)
    {
        _context = context;
    }
    public void SaveContenders(IEnumerable<Contender> contenders, int tryNumber)
    {
        _context.Database.EnsureCreated();
        int contenderNumber = 1;
        foreach (var contender in contenders)
        {
            contender.SequenceNumber = contenderNumber;
            contender.TryId = tryNumber;
            _context.Contenders.Add(contender);
            contenderNumber++;
        }

        _context.SaveChanges();
    }

    public List<Contender> GetContendersByTryId(int tryId)
    {
        return _context.Contenders.Where(c => c.TryId == tryId).ToList();
    }

    public void ClearOldContenders()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.ChangeTracker.Clear();
    }
}