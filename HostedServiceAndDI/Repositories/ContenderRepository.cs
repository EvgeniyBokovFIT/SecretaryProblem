﻿using HostedServiceAndDI.Entities;
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
}