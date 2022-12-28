using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Services;
using HostedServiceAndDI.Strategies;
using Microsoft.Extensions.Hosting;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Entities;

public class Princess : IHostedService
{
    private readonly Hall _hall;

    private readonly FileWriter _fileWriter;
    
    private readonly IPrincessBehaviour _strategy;

    private readonly int _contendersCount = Config.GetContendersCount();
    
    private readonly int _attemptsCount = Config.GetAttemptsCount();

    private readonly ContenderRepository _contenderRepository;

    public Princess(Hall hall, FileWriter fileWriter, IPrincessBehaviour behaviour, ContenderRepository contenderRepository)
    {
        _hall = hall;
        _hall.FillContenders();
        _fileWriter = fileWriter;
        _strategy = behaviour;
        _contenderRepository = contenderRepository;
    }
    
    
    public Contender? ChooseContender()
    {
        try
        {
            for (int i = 0; i < _contendersCount; i++)
            {
                var contender = _hall.GetNextContender();
                if (_strategy.IsChosenContender(contender))
                {
                    return contender;
                }
            }
            
        }
        catch(EmptyHallException)
        {
            return null;
        }

        return null;
    }

    private int GetHappiness(Contender? chosenContender)
    {
        if (chosenContender is null)
        {
            return 10;
        }

        if (chosenContender.Rating.Equals(_contendersCount))
        {
            return 20;
        }
        
        if (chosenContender.Rating.Equals(_contendersCount - 2))
        {
            return 50;
        }
        
        if (chosenContender.Rating.Equals(_contendersCount - 4))
        {
            return 100;
        }

        return 0;
    }

    private void WriteHappinessToFile(Contender? chosenContender)
    {
        int happiness = GetHappiness(chosenContender);

        _fileWriter.WriteHappinessToFile(happiness);
    }

    private int SimulatePrincessBehaviour()
    {
        _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
        Contender? chosenContender = ChooseContender();
        Console.WriteLine($"Chosen contender: {chosenContender}");
        int happiness = GetHappiness(chosenContender);
        Console.WriteLine($"Happiness {happiness}");
        WriteHappinessToFile(chosenContender);
        return happiness;
    }

    public void GenerateAttempts(int attemptsCount)
    {
        double averageHappiness = 0;
        _contenderRepository.ClearOldContenders();
        _hall.FillContenders();
        _strategy.Reset();
        for (var i = 0; i < attemptsCount; i++)
        {
            var enumerableContenders = _hall.Contenders.AsEnumerable();
            _contenderRepository.SaveContenders(enumerableContenders, i + 1);
            _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
            var chosenContender = ChooseContender();
            WriteHappinessToFile(chosenContender);
            averageHappiness += GetHappiness(chosenContender);
            Console.WriteLine($"Chosen contender: {chosenContender}");
            Console.WriteLine($"Happiness: {GetHappiness(chosenContender)}");

            _hall.FillContenders();
            _strategy.Reset();
        }

        averageHappiness /= attemptsCount;
        Console.WriteLine($"Average happiness {averageHappiness}");
    }

    public int SimulateProcessOfChoosingByTryNumber(int tryNumber)
    {
        IEnumerable<Contender> contenders = _contenderRepository.GetContendersByTryId(tryNumber).Result;
        _strategy.Reset();
        //_hall.Contenders.Clear();
        _hall.Contenders = new Queue<Contender>(contenders);
        return SimulatePrincessBehaviour();
    }

    private double GetAverageHappiness()
    {
        double averageHappiness = 0;
        for (int i = 0; i < _attemptsCount; i++)
        {
            averageHappiness += SimulateProcessOfChoosingByTryNumber(i + 1);
        }

        return averageHappiness / _attemptsCount;
    }

    private void DoWork()
    {
        WriteUsage();

        while (true)
        {
            string input = Console.ReadLine() ?? "generate";
            if (input.Equals("generate"))
            {
                GenerateAttempts(_attemptsCount);
                continue;
            }
            if(input.Equals("avg"))
            {
                var averageHappiness = GetAverageHappiness();
                Console.WriteLine($"Average happiness: {averageHappiness}");
                continue;
            }
            if(Int32.TryParse(input, out var attemptNumber))
            {
                SimulateProcessOfChoosingByTryNumber(attemptNumber);
                continue;
            }

            Console.WriteLine("Bad input");
            WriteUsage();
            
        }
    }

    private void WriteUsage()
    {
        Console.WriteLine($"Enter \"generate\" to generate {_attemptsCount} attempts");
        
        Console.WriteLine("Enter \"avg\" to get average value of happiness");
        
        Console.WriteLine($"Enter number of attempt from (1 to {_attemptsCount}) " +
                          "to simulate princess behaviour on this attempt");
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(DoWork);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}