namespace SecretaryProblem;

public class Princess
{
    private readonly Hall _hall;

    private readonly IPrincessBehaviour _strategy;
    
    public Princess(Hall hall, IPrincessBehaviour behaviour)
    {
        _hall = hall;
        _strategy = behaviour;
    }


    public Contender? ChooseContender()
    {
        while (_hall.ContendersCount > 0)
        {
            var contender = _hall.GetNextContender();
            if (_strategy.IsChosenContender(contender))
            {
                return contender;
            }
        }

        return null;
    }
}