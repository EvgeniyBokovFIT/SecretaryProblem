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

    private bool IsContenderFromTheBetterHalf(Contender contender, List<Contender> checkedContenders)
    { 
        return checkedContenders.Count(checkedContender =>
            contender != checkedContender && contender == _friend.Compare(contender, checkedContender)) >= 50;
    }

    private Contender ChooseContenderFromFirstPart (ICollection<Contender> checkedContenders, int contendersCount)
    {
        var bestContender = _hall.GetNextContender();
        checkedContenders.Add(bestContender);
        int iterationsWithoutChanges = 0;
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2; i++)
        {
            var newContender = _hall.GetNextContender();
            bestContender = _friend.Compare(bestContender, newContender);
            if (oldBest != bestContender && iterationsWithoutChanges > 3)
            {
                return bestContender;
            }
            checkedContenders.Add(newContender);
            if (oldBest == bestContender)
            {
                iterationsWithoutChanges++;
                continue;
            }
            iterationsWithoutChanges = 0;
            oldBest = bestContender;
        }

        return bestContender;
    }
    
    public Contender? ChooseContender()
    {
        var checkedContenders = new List<Contender>();
        var contendersCount = _hall.ContendersCount;

        var bestContender = ChooseContenderFromFirstPart(checkedContenders, contendersCount);
        if (checkedContenders.Count < contendersCount / 2)
            return bestContender;
        
        for (int i = contendersCount / 2; i < contendersCount - 1; i++)
        {
            var newContender = _hall.GetNextContender();
            checkedContenders.Add(newContender);
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }

        var lastContender = _hall.GetNextContender();

        return IsContenderFromTheBetterHalf(lastContender, checkedContenders) ? lastContender : null;
    }
}