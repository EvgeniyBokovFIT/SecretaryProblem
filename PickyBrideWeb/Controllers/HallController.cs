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

    private static List<int> _checkedAttempts = new();

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
    public async Task<ContenderDto> NextContender(int tryId, string? session)
    {
        //Console.WriteLine("NEXT");
        if (_hall.Contenders.Count == 0)
        {
            if (_checkedAttempts.Contains(tryId))
            {
                _hall.LastViewedContender = null;
                Console.WriteLine("NOTHING");
                return new ContenderDto
                {
                    Name = null
                };
            }
            _checkedAttempts.Add(tryId);
            
            IEnumerable<Contender> contenders = await _contenderRepository.GetContendersByTryId(tryId);
            _hall.Contenders = new Queue<Contender>(contenders);
        }

        // if (_hall.Contenders.Count == 0)
        // {
        //     return new ContenderDto
        //     {
        //         Name = null
        //     };
        // }
        
        var contender = _hall.GetNextContender();
        _friend.ViewedContenders.Add(contender);
        //Console.WriteLine(contender.Name);
        return new ContenderDto
        {
            Name = contender.Name
        };
    }

    [HttpPost("{tryId}/select")]
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