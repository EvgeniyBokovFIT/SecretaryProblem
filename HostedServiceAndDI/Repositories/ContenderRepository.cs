using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Repositories;

public class ContenderRepository
{
    private readonly EnvironmentContext _context;

    public ContenderRepository(EnvironmentContext context)
    {
        _context = context;
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
    public void SaveContenders(IEnumerable<DbContender> contenders, int tryNumber)
    {
        int contenderNumber = 1;
        foreach (var contender in contenders)
        {
            var dbContender = new DbContender
            {
                Name = contender.Name, Rating = contender.Rating,
                SequenceNumber = contenderNumber, TryId = tryNumber
            };
            _context.DbContenders.Add(dbContender);
            contenderNumber++;
        }

        _context.SaveChanges();
    }

    public List<DbContender> GetUsersByTryId(int tryId)
    {
        return _context.DbContenders.Where(c => c.TryId == tryId).ToList();
    }
}