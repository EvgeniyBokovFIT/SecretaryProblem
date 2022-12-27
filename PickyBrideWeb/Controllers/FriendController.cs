using HostedServiceAndDI.Entities;
using Microsoft.AspNetCore.Mvc;
using Nsu.PeakyBride.DataContracts;

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
    public Contender Compare(int tryId, string? session, [FromBody] CompareDto names)
    {
        try
        {
            var contender1 = _friend
                .ViewedContenders
                .First(c => c.TryId == tryId && c.Name == names.Name1);
            //Console.WriteLine("COMPARE 1 " + contender1);
            var contender2 = _friend
                .ViewedContenders
                .First(c => c.TryId == tryId && c.Name == names.Name2);
            //Console.WriteLine("COMPARE 2 " + contender2);

            var bestContender = _friend.Compare(contender1, contender2);
            //Console.WriteLine("BEST FROM COMPARING " + bestContender);
            return new Contender
            {
                Name = bestContender.Name
            };
        }
        catch (Exception)
        {
            return new Contender
            {
                Name = null
            };
        }
    }
}