using HostedServiceAndDI.Entity;

namespace HostedServiceAndDI.Strategy;

public interface IPrincessBehaviour
{
    public bool IsChosenContender(Contender contender);

    public void Reset();
}