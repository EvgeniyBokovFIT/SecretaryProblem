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
    
    public Contender? ChooseContender()
    {
        var contenders = new List<Contender>();
        int iterationsWithoutChanges = 0;
        var contendersCount = _hall.ContendersCount;
        var bestContender = _hall.GetNextContender();
        contenders.Add(bestContender);
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2; i++)
        {
            var newContender = _hall.GetNextContender();
            contenders.Add(newContender);
            bestContender = _friend.Compare(bestContender, newContender);
            
            if (oldBest != bestContender && iterationsWithoutChanges > 3)
            {
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

        for (int i = contendersCount / 2; i < contendersCount - 1; i++)
        {
            var newContender = _hall.GetNextContender();
            contenders.Add(newContender);
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }

        var lastContender = _hall.GetNextContender();

        return contenders.
            Count(contender => lastContender != contender && lastContender == _friend.Compare(lastContender, contender)) >= 50 ? 
            lastContender : null;
    }
}