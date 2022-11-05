using HostedServiceAndDI.Configuration;

namespace SecretaryProblem.Data;

public class Program
{
    static void Main(string[] args)
    {
        using (var context = new EnvironmentContext())
        {
            context.Database.EnsureCreated();
            for (int i = 0; i < ContenderConfig.GetContendersCount(); i++)
            {
                
            }
        }
    }
}