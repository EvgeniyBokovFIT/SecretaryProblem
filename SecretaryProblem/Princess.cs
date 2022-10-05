namespace SecretaryProblem;

public class Princess
{
    public Contender? ChooseContender(Hall hall, Friend friend)
    {
        int iterationsWithoutChanges = 0;
        var contendersCount = hall.ContendersCount;
        var bestContender = hall.GetNextContender();
        var oldBest = bestContender;
        for (int i = 1; i < contendersCount / 2; i++)
        {
            var newContender = hall.GetNextContender();
            bestContender = friend.Compare(bestContender, newContender);
            if (oldBest == bestContender)
            {
                iterationsWithoutChanges++;
            }
            else
            {
                iterationsWithoutChanges = 0;
                oldBest = bestContender;
            }
            if (iterationsWithoutChanges > 9)
                return bestContender;
        }

        for (int i = contendersCount / 2; i < contendersCount; i++)
        {
            var newContender = hall.GetNextContender();
            var newBestContender = friend.Compare(bestContender, newContender);
            if (newBestContender != bestContender)
                return newBestContender;
        }
        return null;
    }
}