using DataContracts;
using HostedServiceAndDI.Entities;

namespace PickyBrideWeb.Services;

public class FriendService
{
    private readonly Friend _friend;

    public FriendService(Friend friend)
    {
        _friend = friend;
    }
    
    public ContenderDto Compare(int tryId, string? session, CompareDto names)
    {

        var contender1 = _friend
            .ViewedContenders
            .FirstOrDefault(c => c.TryId == tryId && c.Name == names.Name1);
        var contender2 = _friend
            .ViewedContenders
            .FirstOrDefault(c => c.TryId == tryId && c.Name == names.Name2);

        var bestContender = _friend.Compare(contender1, contender2);
        return new ContenderDto
        {
            Name = bestContender.Name
        };

    }
}