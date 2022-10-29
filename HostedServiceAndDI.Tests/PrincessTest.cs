using NUnit.Framework;
using Moq;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class PrincessTest
{
    [Test]
    public void Princess_Pick_Ascending_Sequence_Success()
    {
        Queue<Contender> contenders = new Queue<Contender>();
        for (int i = 0; i < 100; i++)
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
        Assert.That(chosenContender?.Rating, Is.EqualTo(38));
    }

    [Test]
    public void Princess_Pick_Mixed_Sequence_Success()
    {
        Queue<Contender> contenders = new Queue<Contender>();
        for (int i = 0; i < 100; i++)
        {
            if (i != 70 && i != 0)
            {
                contenders.Enqueue(new Contender("", 100 - i));
            }
            else if(i == 70)
            {
                contenders.Enqueue(new Contender("", 100));
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
        
        Assert.That(chosenContender?.Rating, Is.EqualTo(100));
    }
}