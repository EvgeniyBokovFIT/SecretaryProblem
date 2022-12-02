using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Exceptions;
using NUnit.Framework;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class FriendTest
{
    [Test]
    public void Friend_Compare_Success()
    {
        var firstContender = new Contender("", 10);
        var secondContender = new Contender("", 100);
        var friend = new Friend();
        friend.ViewedContenders.Add(firstContender);
        friend.ViewedContenders.Add(secondContender);

        var bestContender = friend.Compare(firstContender, secondContender);
        Assert.That(bestContender, Is.EqualTo(secondContender));
    }
    
    [Test]
    public void Friend_Compare_Unviewed_Contender()
    {
        var firstContender = new Contender("", 10);
        var secondContender = new Contender("", 100);
        var friend = new Friend();
        friend.ViewedContenders.Add(firstContender);

        Assert.Throws<UnviewedContenderException>(() => friend.Compare(firstContender, secondContender));
    }
}