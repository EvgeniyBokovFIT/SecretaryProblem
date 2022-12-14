using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Entities;
using HostedServiceAndDI.Exceptions;
using HostedServiceAndDI.Repositories;
using HostedServiceAndDI.Services;
using HostedServiceAndDI.Strategies;
using Moq;
using NUnit.Framework;
using SecretaryProblem.Data;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class PrincessTest
{
    private readonly int _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? 
                                                      throw new Exception("Setting not found"));
    [Test]
    public void Princess_Pick_Ascending_Sequence_Success()
    {
        Queue<Contender> contenders = GetAscendingSequenceOfContenders();
        var mockHall = new Mock<Hall>();
        mockHall
            .Setup(mh => mh.GetNextContender())
            .Returns(contenders.Dequeue);
        mockHall
            .Setup(mh => mh.ContendersCount)
            .Returns(contenders.Count);

        var mockWriter = new Mock<FileWriter>();
        var mockStrategy = new Mock<MyStrategy>(new Friend());
        var princess = new Princess(mockHall.Object, mockWriter.Object, mockStrategy.Object, 
            new ContenderRepository(new EnvironmentContext()));
        var chosenContender = princess.ChooseContender();
        Assert.That(chosenContender?.Rating, Is.EqualTo(Convert.ToInt32(_contendersCount/2.7 + 1)));
    }

    private Queue<Contender> GetAscendingSequenceOfContenders()
    {
        var contenders = new Queue<Contender>();
        for (int i = 0; i < _contendersCount; i++)
        {
            contenders.Enqueue(new Contender("", i + 1));
        }

        return contenders;
    }

    [Test]
    public void Princess_Pick_Mixed_Sequence_Success()
    {
        Queue<Contender> contenders = GetMixedSequenceOfContenders();
        var mockHall = new Mock<Hall>();
        mockHall
            .Setup(mh => mh.GetNextContender())
            .Returns(contenders.Dequeue);
        mockHall
            .Setup(mh => mh.ContendersCount)
            .Returns(contenders.Count);
        
        var mockWriter = new Mock<FileWriter>();
        var mockStrategy = new Mock<MyStrategy>(new Friend());
        var princess = new Princess(mockHall.Object, mockWriter.Object, 
            mockStrategy.Object, new ContenderRepository(new EnvironmentContext()));
        var chosenContender = princess.ChooseContender();
        
        Assert.That(chosenContender?.Rating, Is.EqualTo(_contendersCount));
    }

    private Queue<Contender> GetMixedSequenceOfContenders()
    {
        Queue<Contender> contenders = new Queue<Contender>();
        for (int i = 0; i < _contendersCount; i++)
        {
            if (i != 70 && i != 0)
            {
                contenders.Enqueue(new Contender("", _contendersCount - i));
                continue;
            }
            if(i == 70)
            {
                contenders.Enqueue(new Contender("", _contendersCount));
                continue;
            }
            
            contenders.Enqueue(new Contender("", 30));
        }

        return contenders;
    }
    
    [Test]
    public void Princess_Empty_Hall_Case_Success()
    {
        var mockHall = new Mock<Hall>();
        mockHall
            .Setup(mh => mh.GetNextContender())
            .Throws(new EmptyHallException(""));

        var mockWriter = new Mock<FileWriter>();
        var mockStrategy = new Mock<MyStrategy>(new Friend());
        var princess = new Princess(mockHall.Object, mockWriter.Object, 
                mockStrategy.Object, new ContenderRepository(new EnvironmentContext()));
        var chosenContender = princess.ChooseContender();
        
        Assert.That(chosenContender, Is.EqualTo(null));
    }
    
}