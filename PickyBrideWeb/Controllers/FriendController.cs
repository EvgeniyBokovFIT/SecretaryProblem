using DataContracts;
using HostedServiceAndDI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PickyBrideWeb.Controllers;

[ApiController]
[Route("freind")]
public class FriendController
{
    private readonly Friend _friend;

    public FriendController(Friend friend)
    {
        _friend = friend;
    }
    
    [HttpPost("{tryId}/compare")]
    public ContenderDto? Compare(int tryId, string? session, [FromBody] CompareDto names)
    {
        try
        {
            var contender1 = _friend
                .ViewedContenders
                .First(c => c.TryId == tryId && c.Name == names.Name1);
            var contender2 = _friend
                .ViewedContenders
                .First(c => c.TryId == tryId && c.Name == names.Name2);
            var bestContender = _friend.Compare(contender1, contender2);
            return new ContenderDto
            {
                Name = bestContender.Name
            };
        }
        catch (Exception)
        {
            return new ContenderDto
            {
                Name = null
            };
        }
    }
}