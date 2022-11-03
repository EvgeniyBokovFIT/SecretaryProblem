using HostedServiceAndDI.Service;
using NUnit.Framework;

namespace HostedServiceAndDI.Tests;

[TestFixture]
public class GeneratorTest
{
    [Test]
    public void Unique_Contenders_Success()
    {
        var generator = new ContenderGenerator();
        var contenders = generator.GenerateContenders();

        var equalNamesCount = contenders
            .GroupBy(c => c.Name)
            .Where(c => c.Count() > 1)
            .Select(c => c)
            .Count();

        Assert.That(equalNamesCount, Is.EqualTo(0));
        
    }
    
}