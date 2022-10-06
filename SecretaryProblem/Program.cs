using SecretaryProblem;

List<Contender> contenders = new ContenderGenerator().GenerateContenders();

var princess = new Princess(new Hall(new Queue<Contender>(contenders)), new Friend());

contenders.ForEach(contender => Console.WriteLine(contender.Name + " " + contender.Rating));

var bestContender = princess.ChooseContender();

var fileWriter = new FileWriter();

if (bestContender is null)
{
    Console.WriteLine(10);
    fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 10);
    return;
}

if (bestContender.Rating > 50)
{
    Console.WriteLine(bestContender.Rating);
    fileWriter.WriteToFile("SecretaryProblem.txt", contenders, bestContender.Rating);
    return;
}

fileWriter.WriteToFile("SecretaryProblem.txt", contenders, 0);
Console.WriteLine(0);


    


