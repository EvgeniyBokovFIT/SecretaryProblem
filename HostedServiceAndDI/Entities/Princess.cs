using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Strategies;
using Microsoft.EntityFrameworkCore;
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
        for (var i = 0; i < triesCount; i++)
        {
            _contenderRepository.SaveContenders(_hall.Contenders, i + 1);
            _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
            var chosenContender = ChooseContender();
            WriteHappinessToFile(chosenContender);
            averageHappiness += GetHappiness(chosenContender);

            _hall.FillContenders();
            _strategy.Reset();
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