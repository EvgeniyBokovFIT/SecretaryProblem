using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.entity;
using HostedServiceAndDI.strategy;
using NUnit.Framework;
using Moq;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class PrincessTest
{
    private readonly int _contendersCount = int.Parse(ConfigProvider.GetConfig()["ContendersCount"] ?? throw new Exception());
    [Test]
    public void Princess_Pick_Ascending_Sequence_Success()
    {
        Queue<Contender> contenders = new Queue<Contender>();
        for (int i = 0; i < _contendersCount; i++)
        {
            contenders.Enqueue(new Contender("", i + 1));
        }
        var mockHall = new Mock<Hall>();
        mockHall
            .Setup(mh => mh.GetNextContender())
            .Returns(contenders.Dequeue);
        mockHall
            .Setup(mh => mh.ContendersCount)
            .Returns(contenders.Count);

        var mockWriter = new Mock<FileWriter>();
        var mockStrategy = new Mock<MyStrategy>(new Friend());
        var princess = new Princess(mockHall.Object, mockWriter.Object, mockStrategy.Object);
        var chosenContender = princess.ChooseContender();
        Assert.That(chosenContender?.Rating, Is.EqualTo(Convert.ToInt32(_contendersCount/2.7 + 1)));
    }

    [Test]
    public void Princess_Pick_Mixed_Sequence_Success()
    {
        Queue<Contender> contenders = new Queue<Contender>();
        for (int i = 0; i < _contendersCount; i++)
        {
            if (i != 70 && i != 0)
            {
                contenders.Enqueue(new Contender("", _contendersCount - i));
            }
            else if(i == 70)
            {
                contenders.Enqueue(new Contender("", _contendersCount));
            }
            else
            {
                contenders.Enqueue(new Contender("", 30));
            }
        }
        var mockHall = new Mock<Hall>();
        mockHall
            .Setup(mh => mh.GetNextContender())
            .Returns(contenders.Dequeue);
        mockHall
            .Setup(mh => mh.ContendersCount)
            .Returns(contenders.Count);

        var mockWriter = new Mock<FileWriter>();
        var mockStrategy = new Mock<MyStrategy>(new Friend());
        var princess = new Princess(mockHall.Object, mockWriter.Object, mockStrategy.Object);
        var chosenContender = princess.ChooseContender();
        
        Assert.That(chosenContender?.Rating, Is.EqualTo(_contendersCount));
    }
}