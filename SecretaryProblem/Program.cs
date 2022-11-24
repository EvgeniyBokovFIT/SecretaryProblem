using SecretaryProblem;

var sum = 0;

var fileWriter = new FileWriter();

for (var i = 0; i < 1000; i++)
{
    var contenders = new ContenderGenerator().GenerateContenders();

    IPrincessBehaviour behaviour = new MyStrategy(new Friend());

    var princess = new Princess(new Hall(new Queue<Contender>(contenders)), behaviour);

    //contenders.ForEach(contender => Console.WriteLine(contender.Name + " " + contender.Rating));
    
    var bestContender = princess.ChooseContender();

    if (bestContender is null)
    {
        Console.WriteLine(10);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 10);
        sum += 10;
        continue;
    }

    if (bestContender.Rating.Equals(100))
    {
        Console.WriteLine(20);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, bestContender.Rating);
        sum += 20;
        continue;
    }
    if (bestContender.Rating.Equals(98))
    {
        Console.WriteLine(50);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, bestContender.Rating);
        sum += 50;
        continue;
    }
    if (bestContender.Rating.Equals(96))
    {
        Console.WriteLine(100);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, bestContender.Rating);
        sum += 100;
        continue;
    }

    fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 0);
    Console.WriteLine(0);
}

var avg = sum / 1000d;
Console.WriteLine($"AVG = {avg}");