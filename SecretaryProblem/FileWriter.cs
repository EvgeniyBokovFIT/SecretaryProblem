namespace SecretaryProblem;

public class FileWriter
{
    public void WriteToFile(in string fileName, in List<Contender> contenders, in int result)
    {
        var names = new List<string>();
        contenders.ForEach(contender => names.Add(contender.Name));
        File.WriteAllLines(fileName, names);
        File.AppendAllText(fileName, result.ToString());
    }
}