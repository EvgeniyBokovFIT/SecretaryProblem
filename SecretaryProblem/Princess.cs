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
        int iterationsWithoutChanges = 0;
        var contendersCount = _hall.ContendersCount;
        var bestContender = _hall.GetNextContender();
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2; i++)
        {
            var newContender = _hall.GetNextContender();
            bestContender = _friend.Compare(bestContender, newContender);
            
            if (oldBest != bestContender && iterationsWithoutChanges > 5)
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

        for (int i = contendersCount / 2; i < contendersCount; i++)
        {
            var newContender = _hall.GetNextContender();
            var newBestContender = _friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }
        return null;
    }
}