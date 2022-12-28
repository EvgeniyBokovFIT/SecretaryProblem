using DataContracts;
using Microsoft.AspNetCore.Mvc;
using PickyBrideWeb.Services;

namespace PickyBrideWeb.Controllers;

[ApiController]
[Route("hall")]
public class HallController
{
    private readonly HallService _hallService;
    public HallController(HallService hallService)
    {
        _hallService = hallService;
    }
    
    [HttpPost("reset")]
    public void Reset(string? session)
    {
        _hallService.Reset(session);   
    }

    [HttpPost("{tryId}/next")]
    public async Task NextContender(int tryId, string? session)
    {
        await _hallService.NextContender(tryId, session);
    }

    [HttpPost("{tryId}/select")]
    public RatingDto GetRating(int tryId, string? session)
    {
        return _hallService.GetRating(tryId, session);
    }
}