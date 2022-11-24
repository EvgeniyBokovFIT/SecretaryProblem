using HostedServiceAndDI.Entities;

namespace HostedServiceAndDI.Strategies;

public interface IPrincessBehaviour
{
    public bool IsChosenContender(Contender contender);

    public void Reset();
}