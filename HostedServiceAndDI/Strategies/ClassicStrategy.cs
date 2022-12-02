using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Strategies;

public class ClassicStrategy : IPrincessBehaviour
{
    private readonly Friend _friend;

    private readonly int _contendersCount;

    private Contender _bestContender;
    
    public ClassicStrategy(Friend friend)
    {
        _friend = friend;
        _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? throw new Exception());
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
            _bestContender = _friend.Compare(_bestContender, contender);
            return false;
        }

        Contender oldContender = _bestContender;
        _bestContender = _friend.Compare(_bestContender, contender);
        if (_bestContender != oldContender)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _bestContender = new Contender("", 0);
        _friend.ViewedContenders.Clear();
    }
}