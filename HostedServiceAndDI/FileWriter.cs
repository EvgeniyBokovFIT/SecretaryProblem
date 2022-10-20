namespace HostedServiceAndDI;

public class FileWriter
{
    private readonly string _filename = "SecretaryProblem.txt";
    
    public void WriteContendersNamesToFile(in List<string> contendersNames)
    {
        File.WriteAllLines(_filename, contendersNames);
    }

    public void WriteHappinessToFile(in int happiness)
    {
        File.AppendAllText(_filename, happiness.ToString());
    }
    
}