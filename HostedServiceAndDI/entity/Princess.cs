﻿using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Strategy;
using Microsoft.Extensions.Hosting;

namespace HostedServiceAndDI.Entity;

public class Princess : IHostedService
{
    private readonly Hall _hall;

    private readonly FileWriter _fileWriter;
    
    private readonly IPrincessBehaviour _strategy;

    private readonly int _contendersCount;

    public Princess(Hall hall, FileWriter fileWriter, IPrincessBehaviour behaviour)
    {
        _hall = hall;
        _fileWriter = fileWriter;
        _strategy = behaviour;
        _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? throw new Exception());
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
            Console.WriteLine("HallExc");
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

        if (chosenContender.Rating > 50)
        {
            return chosenContender.Rating;
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
        WriteHappinessToFile(chosenContender);
    }

    private void DoSeveralTries(int triesCount)
    {
        double averageHappiness = 0;
        for (int i = 0; i < triesCount; i++)
        {
            _hall.FillContenders();
            _strategy.Reset();
            
            _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
            Contender? chosenContender = ChooseContender();
            WriteHappinessToFile(chosenContender);
            averageHappiness += GetHappiness(chosenContender);
        }

        averageHappiness /= triesCount;
        Console.WriteLine($"Average happiness {averageHappiness}");
    }
    
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Princess StartAsync");
        Task.Run(() => DoSeveralTries(100));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Princess StopAsync");
        return Task.CompletedTask;
    }
}