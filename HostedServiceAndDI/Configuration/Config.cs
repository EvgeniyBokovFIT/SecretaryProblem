namespace HostedServiceAndDI.Configuration;

public static class Config
{
    public static int GetContendersCount()
    {
        var configManager = ConfigProvider.GetConfig();
        return int.Parse(configManager["ContendersCount"] ?? throw new Exception("Setting not found"));
    }

    public static int GetAttemptsCount()
    {
        var configManager = ConfigProvider.GetConfig();
        return int.Parse(configManager["AttemptsCount"] ?? throw new Exception("Setting not found"));
    }
}