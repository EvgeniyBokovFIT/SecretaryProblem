﻿using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Strategies;
using HostedServiceAndDI.Utils;
using Microsoft.Extensions.Hosting;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Entities;

public class Princess : IHostedService
{
    private readonly Hall _hall;

    private readonly FileWriter _fileWriter;
    
    private readonly IPrincessBehaviour _strategy;

    private readonly int _contendersCount;

    private readonly ContenderRepository _contenderRepository;

    public Princess(Hall hall, FileWriter fileWriter, IPrincessBehaviour behaviour, ContenderRepository contenderRepository)
    {
        _hall = hall;
        _fileWriter = fileWriter;
        _strategy = behaviour;
        _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? throw new Exception());
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

    private void DoOneTry()
    {
        _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
        Contender? chosenContender = ChooseContender();
        Console.WriteLine($"Chosen contender: {chosenContender}");
        Console.WriteLine($"Happiness {GetHappiness(chosenContender)}");
        WriteHappinessToFile(chosenContender);
    }

    private void DoSeveralTries(int triesCount)
    {
        double averageHappiness = 0;
        _contenderRepository.ClearOldContenders();
        for (var i = 0; i < triesCount; i++)
        {
            var enumerableContenders = _hall.Contenders.AsEnumerable();

            _contenderRepository.SaveContenders(enumerableContenders, i + 1);
            _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
            var chosenContender = ChooseContender();
            WriteHappinessToFile(chosenContender);
            averageHappiness += GetHappiness(chosenContender);
            Console.WriteLine("Choosen contender: " + chosenContender);
            Console.WriteLine(GetHappiness(chosenContender));

            _hall.FillContenders();
            _strategy.Reset();
        }

        averageHappiness /= triesCount;
        Console.WriteLine($"Average happiness {averageHappiness}");
    }

    private void SimulateProcessOfChoosingByTryNumber(int tryNumber)
    {
        IEnumerable<Contender> contenders = _contenderRepository.GetContendersByTryId(tryNumber);
        _strategy.Reset();
        _hall.Contenders.Clear();
        _hall.Contenders = new Queue<Contender>(contenders);
        DoOneTry();
    }

    private void DoWork()
    {
        Console.WriteLine("Enter \"Generate\" to generate 100 tries");
        Console.WriteLine("Enter number of attempt from (1 to 100) to simulate princess behaviour on this attempt");

        while (true)
        {
            string input = Console.ReadLine() ?? "Generate";
            if (input.Equals("Generate"))
            {
                DoSeveralTries(100);
            }
            else
            {
                int tryNumber = Int32.Parse(input);
                SimulateProcessOfChoosingByTryNumber(tryNumber);
            }
        }
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