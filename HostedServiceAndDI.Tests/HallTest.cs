using HostedServiceAndDI.entity;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.service;
using NUnit.Framework;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class HallTest
{
    [Test]
    public void Hall_Get_Next_Contender_Success()
    {
        var hall = new Hall(new ContenderGenerator());
        var contender = hall.GetNextContender();
        
        Assert.IsInstanceOf(typeof(Contender), contender);
    }

    [Test]
    public void Empty_Hall_Case_Failure()
    {
        var hall = new Hall(new ContenderGenerator());
        var contendersCount = hall.ContendersCount;

        for (int i = 0; i < contendersCount; i++)
        {
            hall.GetNextContender();
        }

        Assert.Throws<EmptyHallException>(() => hall.GetNextContender());
    }
}