namespace SecretaryProblem;

public class Princess
{
    private readonly Hall _hall;

    private readonly Friend _friend;

    public Princess(Hall hall, Friend friend)
    {
        _friend = friend;
        _hall = hall;
    }

    private Contender ChooseContenderFromFirstPart (ICollection<Contender> checkedContenders, int contendersCount, out bool bestChosen)
    {
        var bestContender = _hall.GetNextContender();
        checkedContenders.Add(bestContender);
        int iterationsWithoutChanges = 0;
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2; i++)
        {
            var newContender = _hall.GetNextContender();
            checkedContenders.Add(newContender);
            bestContender = _friend.Compare(bestContender, newContender);
            
            if (oldBest != bestContender && iterationsWithoutChanges > 3)
            {
                bestChosen = true;
                return bestContender;
            }

            if (oldBest == bestContender)
            {
                iterationsWithoutChanges++;
            }
            else
            {
                iterationsWithoutChanges = 0;
                oldBest = bestContender;
            }
            
        }

        bestChosen = false;
        return bestContender;
    }
    
    public Contender? ChooseContender()
    {
        var checkedContenders = new List<Contender>();
        var contendersCount = _hall.ContendersCount;

        var bestContender = ChooseContenderFromFirstPart(checkedContenders, contendersCount, out var bestChosen);

        if (bestChosen)
        {
            return bestContender;
        }

        for (int i = contendersCount / 2; i < contendersCount - 1; i++)
        {
            var newContender = _hall.GetNextContender();
            checkedContenders.Add(newContender);
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }

        var lastContender = _hall.GetNextContender();

        return checkedContenders.
            Count(contender => lastContender != contender && lastContender == _friend.Compare(lastContender, contender)) >= 50 ? 
            lastContender : null;
    }
}