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
        for (int i = 1; i < contendersCount / 2; i++)
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
    
    public Contender? ChooseContender()
    {
        var contendersCount = _hall.ContendersCount;

        var bestContender = ChooseContenderFromFirstPart(contendersCount, out var contenderChosen);
        if (contenderChosen)
            return bestContender;
        
        for (int i = contendersCount / 2; i < contendersCount - 1; i++)
        {
            var newContender = _hall.GetNextContender();
            _friend.ViewedContenders.Add(newContender);
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }

        var lastContender = _hall.GetNextContender();

        return IsContenderFromTheBetterHalf(lastContender) ? lastContender : null;
    }
}