using DataContracts;
using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Repositories;
using Microsoft.AspNetCore.Mvc;
using SecretaryProblem.Data;

namespace PickyBrideWeb.Controllers;

[ApiController]
[Route("hall")]
public class HallController
{
    private readonly Hall _hall;
    private readonly ContenderRepository _contenderRepository;
    private readonly Friend _friend;

    private List<int> _checkedAttempts = new();

    public HallController(Hall hall, ContenderRepository contenderRepository, Friend friend)
    {
        _hall = hall;
        _contenderRepository = contenderRepository;
        _friend = friend;
    }
    
    [HttpPost("reset")]
    public void Reset(string? session)
    {
        _hall.Contenders.Clear();
        _friend.ViewedContenders.Clear();
    }

    [HttpPost("{tryId}/next")]
    public ContenderDto? NextContender(int tryId, string? session)
    {
        if (_hall.Contenders.Count == 0)
        {
            if (_checkedAttempts.Contains(tryId))
            {
                return new ContenderDto
                {
                    Name = null
                };
            }
            _checkedAttempts.Add(tryId);
            IEnumerable<Contender> contenders = _contenderRepository.GetContendersByTryId(tryId);
            _hall.Contenders = new Queue<Contender>(contenders);
        }
        var contender = _hall.GetNextContender();
        _friend.ViewedContenders.Add(contender);
        return new ContenderDto
        {
            Name = contender.Name
        };
    }

    [HttpPost("{tryId}/select")]
    public RatingDto GetRating(int tryId, string? session)
    {
        return new RatingDto
        {
            Rank = _hall.LastViewedContender.Rating
        };
    }
}