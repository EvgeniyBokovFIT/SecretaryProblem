using DataContracts;
using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Repositories;
using SecretaryProblem.Data;

namespace PickyBrideWeb.Services;

public class HallService
{
    private readonly Hall _hall;
    private readonly ContenderRepository _contenderRepository;
    private readonly Friend _friend;
    private readonly int _attemptsCount = Config.GetAttemptsCount();

    private static List<int> _checkedAttempts = new();

    public HallService(Hall hall, ContenderRepository contenderRepository, Friend friend)
    {
        _hall = hall;
        _contenderRepository = contenderRepository;
        _friend = friend;
    }
    
    public void Reset(string? session)
    {
        _contenderRepository.ClearOldContenders();
        _hall.FillContenders();
        for (var i = 0; i < 100; i++)
        {
            var enumerableContenders = _hall.Contenders.AsEnumerable();
            _contenderRepository.SaveContenders(enumerableContenders, i + 1);

            _hall.FillContenders();
        }
        _hall.Contenders.Clear();
    }
    
    public async Task<ContenderDto> NextContender(int tryId, string? session)
    {
        try
        {
            if (_hall.Contenders.Count == 0)
            {
                if (_checkedAttempts.Contains(tryId))
                {
                    _hall.LastViewedContender = null;
                    return new ContenderDto
                    {
                        Name = null
                    };
                }

                _checkedAttempts.Add(tryId);

                IEnumerable<Contender> contenders = await _contenderRepository.GetContendersByTryId(tryId);
                _hall.Contenders = new Queue<Contender>(contenders);
            }

            var contender = _hall.GetNextContender();
            _friend.ViewedContenders.Add(contender);
            return new ContenderDto
            {
                Name = contender.Name
            };
        }
        catch (EmptyHallException e)
        {
            return new ContenderDto
            {
                Name = null
            };
        }
    }
    
    public RatingDto GetRating(int tryId, string? session)
    {
        _hall.Contenders.Clear();
        if (_hall.LastViewedContender is null)
        {
            return new RatingDto
            {
                Rank = null
            };
        }

        Console.WriteLine("SELECT");
        Console.WriteLine(_hall.LastViewedContender.Name + " " + _hall.LastViewedContender.TryId);

        return new RatingDto
        {
            Rank = _hall.LastViewedContender.Rating
        };
    }
}