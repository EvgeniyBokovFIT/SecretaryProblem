using SecretaryProblem.Data;

namespace HostedServiceAndDI.Repositories;

public class ContenderRepository
{
    private readonly EnvironmentContext _context;

    public ContenderRepository(EnvironmentContext context)
    {
        _context = context;
        _context.Database.EnsureDeleted();
    }
    public void SaveContenders(IEnumerable<Contender> contenders, int tryNumber)
    {
        _context.Database.EnsureCreated();
        int contenderNumber = 1;
        foreach (var contender in contenders)
        {
            var dbContender = new Contender
            {
                Name = contender.Name, Rating = contender.Rating,
                SequenceNumber = contenderNumber, TryId = tryNumber
            };
            _context.DbContenders.Add(dbContender);
            contenderNumber++;
        }

        _context.SaveChanges();
    }

    public List<Contender> GetContendersByTryId(int tryId)
    {
        return _context.DbContenders.Where(c => c.TryId == tryId).ToList();
    }

    public void ClearOldContenders()
    {
        _context.Database.EnsureDeleted();
    }
}