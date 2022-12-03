﻿using Microsoft.EntityFrameworkCore;
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
            _context.DbContenders.Add(contender);
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
        _context.Database.EnsureCreated();
    }
}