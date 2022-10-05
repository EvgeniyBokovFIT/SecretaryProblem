using SecretaryProblem;

Princess princess = new Princess();
List<Contender> contenders = new ContenderGenerator().GenerateContenders();
int i = 0;
foreach (var contender in contenders)
{
    Console.WriteLine(i + " " + contender.Name + " " + contender.Rating);
    i++;
}
var bestContender = princess.ChooseContender(new Hall(contenders), new Friend());
if (bestContender is null)
{
    Console.WriteLine(10);
    return;
}

if (bestContender.Rating > 50)
{
    Console.WriteLine(bestContender.Rating);
    return;
}
Console.WriteLine(0);


    


