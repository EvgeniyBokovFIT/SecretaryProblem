using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;

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
        if (_bestContender != oldBest && _iterationsWithoutChanges > 7)
        {
            return true;
        }

        if (oldBest == _bestContender)
        {
            _iterationsWithoutChanges++;
            return false;
        }

        _iterationsWithoutChanges = 0;
        return false;
    }

    private bool IsChosenContenderFromLastPart(Contender contender)
    {
        var oldBest = _bestContender;
        _bestContender = _friend.Compare(_bestContender, contender);
        if (_friend.ViewedContenders.Count == _contendersCount)
        {
            if (IsContenderGivePoints(contender))
            {
                return true;
            }

            return false;
        }
        if(_bestContender != oldBest)
        {
            return true;
        }
        return false;
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