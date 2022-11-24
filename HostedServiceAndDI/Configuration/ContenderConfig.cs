namespace HostedServiceAndDI.Configuration;

public static class ContenderConfig
{
    public static int GetContendersCount()
    {
        var configManager = ConfigProvider.GetConfig();
        return int.Parse(configManager["ContendersCount"] ?? throw new Exception("Setting not found"));
    }
}