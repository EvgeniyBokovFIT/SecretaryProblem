using Microsoft.Extensions.Configuration;

namespace HostedServiceAndDI.Configuration;

public static class ConfigProvider
{
    public static ConfigurationManager GetConfig()
    {
        var configManager = new ConfigurationManager();
        configManager
            .AddJsonFile("C:/Users/Evgeniy/RiderProjects/SecretaryProblem/HostedServiceAndDI/configuration/appsettings.json");
        return configManager;
    }
}