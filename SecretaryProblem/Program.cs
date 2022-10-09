using SecretaryProblem;

var sum = 0;

var fileWriter = new FileWriter();

for (var i = 0; i < 1000; i++)
{
    var contenders = new ContenderGenerator().GenerateContenders();

    var princess = new Princess(new Hall(new Queue<Contender>(contenders)), new Friend());

    //contenders.ForEach(contender => Console.WriteLine(contender.Name + " " + contender.Rating));
    
    var bestContender = princess.ChooseContender();

    if (bestContender is null)
    {
        Console.WriteLine(10);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 10);
        sum += 10;
        continue;
    }

    if (bestContender.Rating > 50)
    {
        Console.WriteLine(bestContender.Rating);
        fileWriter.WriteToFile("SecretaryProblem.txt", contenders, bestContender.Rating);
        sum += bestContender.Rating;
        continue;
    }

    fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 0);
    Console.WriteLine(0);
}

var avg = sum / 1000d;
Console.WriteLine($"AVG = {avg}");