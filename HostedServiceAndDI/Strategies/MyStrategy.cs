using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Strategies;

public class MyStrategy: IPrincessBehaviour
{
    private readonly Friend _friend;

    private int _iterationsWithoutChanges;

    private readonly int _contendersCount;

    private Contender _bestContender;

    public MyStrategy(Friend friend)
    {
        _friend = friend;
        _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? throw new Exception());
        _bestContender = new Contender("", 0);
    }

    public void Reset()
    {
        _bestContender = new Contender("", 0);
        _friend.ViewedContenders.Clear();
        _iterationsWithoutChanges = 0;
    }
    
    public bool IsChosenContender(Contender contender)
    {
        _friend.ViewedContenders.Add(contender);
        if (_friend.ViewedContenders.Count == 1)
        {
            _bestContender = contender;
            return false;
        }
        if (_friend.ViewedContenders.Count < _contendersCount / 2)
        {
            return IsChosenContenderFromFirstPart(contender);
        }

        return IsChosenContenderFromLastPart(contender);
    }

    private bool IsChosenContenderFromFirstPart(Contender contender)
    {
        var oldBest = _bestContender;
        _bestContender = _friend.Compare(_bestContender, contender);
        // if (_bestContender != oldBest && _iterationsWithoutChanges > 30)
        // {
        //     Console.WriteLine("AAAAAA " + contender.Rating);
        //     return true;
        // }
        return ContenderIsBetterThanPrevious(30);

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
        if (_friend.ViewedContenders.Count == _contendersCount)
        {
            if (IsContenderGivePoints(contender))
            {
                return true;
            }

            return false;
        }

        _bestContender = _friend.Compare(_bestContender, contender);

        return ContenderIsBetterThanPrevious(Convert.ToInt32(_friend.ViewedContenders.Count * 0.95));
    }

    private bool ContenderIsBetterThanPrevious(int numOfContenders)
    {
        int viewedContendersCount = _friend.ViewedContenders.Count;
        
        Contender contender = _friend.ViewedContenders[viewedContendersCount - 1];
        var contenderBetterThan = _friend.ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == _friend.Compare(contender, checkedContender));
        
        // if (viewedContendersCount < 42)
        // {
        //     return false;
        // }

        if (contenderBetterThan < viewedContendersCount - 5 || contenderBetterThan == viewedContendersCount - 2)
        {
            return false;
        }
        
        if (contenderBetterThan is > 92 and < 95 && viewedContendersCount > 95 
                                                 && contenderBetterThan != viewedContendersCount - 4
                                                 && contenderBetterThan != viewedContendersCount - 2)
        {
            return true;
        }

        if (viewedContendersCount < 60 && contenderBetterThan < viewedContendersCount - 2)
        {
            return false;
        }
        
        
        if (viewedContendersCount < 80 && (contenderBetterThan < viewedContendersCount - 3 
                                           || contenderBetterThan == viewedContendersCount - 1))
        {
            return false;
        }

        if (viewedContendersCount >= 95 && contenderBetterThan == viewedContendersCount - 4)
        {
            return false;
        }

        if (contenderBetterThan >= numOfContenders)
        {
            return true;
        }

        return false;
    }
    
    private bool IsContenderGivePoints(Contender contender)
    {
        var lastContenderBetterThan = _friend.ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == _friend.Compare(contender, checkedContender));
        var lastContenderRating = lastContenderBetterThan + 1;
        if (lastContenderRating is 100 or 98 or 96)
        {
            return true;
        }

        return false;
    }
}