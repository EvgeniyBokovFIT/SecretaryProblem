using Microsoft.Extensions.Hosting;

namespace HostedServiceAndDI;

public class Princess : IHostedService
{
    private readonly Hall _hall;

    private readonly FileWriter _fileWriter;
    
    private readonly IPrincessBehaviour _strategy;

    public Princess(Hall hall, FileWriter fileWriter, IPrincessBehaviour behaviour)
    {
        _hall = hall;
        _fileWriter = fileWriter;
        _strategy = behaviour;
    }
    
    
    public Contender? ChooseContender()
    {
        while (_hall.ContendersCount > 0)
        {
            var contender = _hall.GetNextContender();
            if (_strategy.IsChosenContender(contender))
            {
                return contender;
            }
        }

        return null;
    }

    private void WriteHappinessToFile(Contender? chosenContender)
    {
        if (chosenContender is null)
        {
            _fileWriter.WriteHappinessToFile(10);
            Console.WriteLine(10);
            return;
        }

        if (chosenContender.Rating > 50)
        {
            _fileWriter.WriteHappinessToFile(chosenContender.Rating);
            Console.WriteLine($"Happiness = {chosenContender.Rating}");
            return;
        }
        
        _fileWriter.WriteHappinessToFile(0);
        Console.WriteLine(0);

    }

    private void DoWork()
    {
        _fileWriter.WriteContendersNamesToFile(_hall.ContendersNames);
        Contender? chosenContender = ChooseContender();
        WriteHappinessToFile(chosenContender);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Princess StartAsync");
        Task.Run(DoWork);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Princess StopAsync");
        return Task.CompletedTask;
    }
}