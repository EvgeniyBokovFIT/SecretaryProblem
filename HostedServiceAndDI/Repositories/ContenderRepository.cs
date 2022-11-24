using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Repositories;

public class ContenderRepository
{
    private readonly EnvironmentContext _context;

    public ContenderRepository(EnvironmentContext context)
    {
        _context = context;
    }
    public void SaveContenders(IEnumerable<DbContender> contenders, int tryNumber)
    {
        _context.Database.EnsureCreated();
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

    public List<DbContender> GetDbContendersByTryId(int tryId)
    {
        Console.WriteLine(tryId);
        Console.WriteLine(_context.DbContenders.Count(c => c.TryId == tryId));
        return _context.DbContenders.Where(c => c.TryId == tryId).ToList();
    }
}