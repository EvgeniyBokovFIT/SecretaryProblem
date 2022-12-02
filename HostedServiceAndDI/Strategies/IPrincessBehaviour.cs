using HostedServiceAndDI.Entities;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Strategies;

public interface IPrincessBehaviour
{
    public bool IsChosenContender(Contender contender);

    public void Reset();
}