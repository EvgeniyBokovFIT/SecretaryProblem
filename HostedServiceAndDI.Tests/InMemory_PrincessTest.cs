using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Services;
using HostedServiceAndDI.Strategies;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class InMemory_PrincessTest
{
    private readonly int _contendersCount = Config.GetContendersCount();
    
    private readonly int _attemptsCount = Config.GetAttemptsCount();

    private ContenderRepository _repository;

    private EnvironmentContext _context;

    public InMemory_PrincessTest()
    {

        var options = new DbContextOptionsBuilder<EnvironmentContext>()
            .UseInMemoryDatabase("EnvironmentDatabase")
            .Options;
        
        _context = new EnvironmentContext(options);
    }

    [SetUp]
    public void SetUp()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.ChangeTracker.Clear();

        for (int i = 1; i <= _contendersCount; i++)
        {
            _context.Contenders.Add(new Contender { Name = "", Rating = 100 - i, SequenceNumber = i, TryId = 1 });
        }
        
        _context.SaveChanges();

        _repository = new ContenderRepository(_context);
    }

    [Test]
    public void Simulate_Process_Of_Choosing()
    {
        var writer = new FileWriter();
        var hall = new Hall();
        var strategy = new MyStrategy(new Friend());
        var princess = new Princess(hall, writer, strategy, _repository);
        int happiness = princess.SimulateProcessOfChoosingByTryNumber(1);

        Assert.That(happiness, Is.EqualTo(10));
    }

    [Test]
    public void Generate_Contenders_Test()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.ChangeTracker.Clear();
        
        var generator = new ContenderGenerator();
        var writer = new FileWriter();
        var hall = new Hall(generator);
        var strategy = new MyStrategy(new Friend());
        var princess = new Princess(hall, writer, strategy, _repository);
        princess.GenerateAttempts(_attemptsCount);
        
        Assert.That(_context.Contenders.Count(), Is.EqualTo(10_000));
    }
}
    