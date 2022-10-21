using Microsoft.Extensions.Hosting;

namespace HostedServiceAndDI;

public class Princess : IHostedService
{
    private readonly Hall _hall;

    private readonly Friend _friend;

    private readonly FileWriter _fileWriter;

    public Princess(Hall hall, Friend friend, FileWriter fileWriter)
    {
        _friend = friend;
        _hall = hall;
        _fileWriter = fileWriter;
    }

    private bool IsContenderFromTheBetterHalf(Contender contender)
    { 
        return _friend.ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == _friend.Compare(contender, checkedContender)) >= 50;
    }

    private Contender ChooseContenderFromFirstPart (int contendersCount, out bool contenderChosen)
    {
        var bestContender = _hall.GetNextContender();
        _friend.ViewedContenders.Add(bestContender);
        int iterationsWithoutChanges = 0;
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2.7; i++)
        {
            var newContender = _hall.GetNextContender();
            _friend.ViewedContenders.Add(newContender);
            bestContender = _friend.Compare(bestContender, newContender);
            if (oldBest != bestContender && iterationsWithoutChanges > 3)
            {
                contenderChosen = true;
                return bestContender;
            }
            
            if (oldBest == bestContender)
            {
                iterationsWithoutChanges++;
                continue;
            }
            iterationsWithoutChanges = 0;
            oldBest = bestContender;
        }

        contenderChosen = false;
        return bestContender;
    }
    
    private Contender? ChooseContender()
    {
        var contendersCount = _hall.ContendersCount;

        var bestContender = ChooseContenderFromFirstPart(contendersCount, out var contenderChosen);
        if (contenderChosen)
            return bestContender;
        
        for (int i = (int) (contendersCount / 2.7) + 1; i < contendersCount - 1; i++)
        {
            var newContender = _hall.GetNextContender();
            _friend.ViewedContenders.Add(newContender);
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }

        var lastContender = _hall.GetNextContender();
        _friend.ViewedContenders.Add(lastContender);

        return IsContenderFromTheBetterHalf(lastContender) ? lastContender : null;
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
            Console.WriteLine(chosenContender.Rating);
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