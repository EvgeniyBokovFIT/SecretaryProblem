using HostedServiceAndDI.Configuration;

namespace HostedServiceAndDI.Services;

public class FileWriter
{
    private readonly string _filename;

    public FileWriter()
    {
        var configManager = ConfigProvider.GetConfig();
        _filename = configManager["ReportFile"] ?? throw new Exception("Setting not found");
    }
    public void WriteContendersNamesToFile(in List<string>? contendersNames)
    {
        if (contendersNames != null)
        {
            File.WriteAllLines(_filename, contendersNames);
        }
    }

    public void WriteHappinessToFile(in int? happiness)
    {
        if (happiness != null)
        {
            File.AppendAllText(_filename, happiness.ToString());
        }
    }
}