namespace SecretaryProblem;

public class MyStrategy: IPrincessBehaviour
{
    private readonly Friend _friend;

    private int _iterationsWithoutChanges;

    private readonly int _contendersCount;

    private Contender _bestContender;

    public MyStrategy(Friend friend)
    {
        _friend = friend;
        _contendersCount = 100;
        _bestContender = new Contender("", 0);
    }
    
    public bool IsChosenContender(Contender contender)
    {
        _friend.ViewedContenders.Add(contender);
        if (_friend.ViewedContenders.Count == 1)
        {
            _bestContender = contender;
            return false;
        }
        if (_friend.ViewedContenders.Count < _contendersCount / 2.7)
        {
            return IsChosenContenderFromFirstPart(contender);
        }

        return IsChosenContenderFromLastPart(contender);
    }

    private bool IsChosenContenderFromFirstPart(Contender contender)
    {
        var oldBest = _bestContender;
        _bestContender = _friend.Compare(_bestContender, contender);
        if (_bestContender != oldBest && _iterationsWithoutChanges > 30)
        {
            Console.WriteLine("AAAAAA " + _bestContender.Rating);
            return true;
        }

        if (oldBest == _bestContender)
        {
            _iterationsWithoutChanges++;
        }
        else
        {
            _iterationsWithoutChanges = 0;
        }

        return false;
    }

    private bool IsChosenContenderFromLastPart(Contender contender)
    {
        var oldBest = _bestContender;
        _bestContender = _friend.Compare(_bestContender, contender);
        if (_friend.ViewedContenders.Count == 100)
        {
            if (IsContenderGivePoints(contender))
            {
                Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBB");
                return true;
            }

            return false;
        }
        if (_bestContender != oldBest && _iterationsWithoutChanges > 15)
        {
            Console.WriteLine("2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa2 " + _bestContender.Rating);
            return true;
        }
        if (oldBest == _bestContender)
        {
            _iterationsWithoutChanges++;
        }
        else
        {
            _iterationsWithoutChanges = 0;
        }
        
        return ContenderIsBetterThanFewPrevious(30);
    }

    private bool ContenderIsBetterThanFewPrevious(int numOfContenders)
    {
        int viewedContendersCount = _friend.ViewedContenders.Count;
        if (viewedContendersCount <= numOfContenders)
        {
            return false;
        }
        Contender contender = _friend.ViewedContenders[viewedContendersCount - 1];
        for (int i = 0; i < numOfContenders; i++)
        {
            if (//i <= viewedContendersCount - 2 &&
                contender != _friend.Compare(contender, _friend.ViewedContenders[viewedContendersCount - 2 - i]))
            {
                return false;
            }
        }

        Console.WriteLine("FEWFEWFEWFEWFEW " + contender.Rating);
        return true;
    }
    
    private bool IsContenderGivePoints(Contender contender)
    {
        var lastContenderBetterThan = _friend.ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == _friend.Compare(contender, checkedContender));
        if (lastContenderBetterThan == 99 || lastContenderBetterThan == 97 || lastContenderBetterThan == 95)
        {
            return true;
        }

        return false;
    }
}