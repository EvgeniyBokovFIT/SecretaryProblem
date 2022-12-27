using System.Text.Json;
using MassTransit;
using Nsu.PeakyBride.DataContracts;

namespace Consumer;

public class PrincessClient : IConsumer<Contender>
{
    private static StrategyClient? _strategy;
    
    private readonly int _contendersCount = 100;
    
    private readonly int _attemptsCount = 100;
    
    private HttpClient _httpClient = new();

    private JsonSerializerOptions _options;

    private static int _tryId = 1;

    private static int avg;

    private static int i;

    public PrincessClient()
    {
        if (_strategy is null)
        {
            _strategy = new StrategyClient();
        }
        _httpClient.BaseAddress = new Uri("https://nsupeakybrideapi20221215134314.azurewebsites.net/api/hall/");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

    }

    private async Task GetNextContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/next?sessionId=7re-qnd-5pu-hld", null);
        //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        
    }

    private async Task SelectContender(int tryId)
    {
        await _httpClient.PostAsync($"{tryId}/select?sessionId=7re-qnd-5pu-hld", null);
    }

    private async Task Reset()
    {
        await _httpClient.PostAsync("reset", null);
    }

    private int GetHappiness(int? rating)
    {
        if (rating is null)
        {
            return 10;
        }

        if (rating == _contendersCount)
        {
            return 20;
        }
        
        if (rating == _contendersCount - 2)
        {
            return 50;
        }
        
        if (rating == _contendersCount - 4)
        {
            return 100;
        }

        return 0;
    }

    public Task Consume(ConsumeContext<Contender> context)
    {
        Console.WriteLine("GOT MESSAGE");
        var contender = context.Message;
        i++;
        Console.WriteLine(contender.Name + " " + _tryId + " " + i);
        
        if (contender.Name is null || _strategy.IsChosenContender(contender, _tryId))
        {
            //Console.WriteLine("FROM CHOOSE CONT");
            //Console.WriteLine(contender.Name);
            SelectContender(_tryId);
            if (_tryId == 100)
            {
                Console.WriteLine(avg/100.0);
            }
            _tryId++;
            _strategy.Reset();
            i = 0;
        }

        if (_tryId <= 100)
        {
            Console.WriteLine("SENDING REQUEST");
            GetNextContender(_tryId);
        }
        
        return Task.CompletedTask;
    }
    
}