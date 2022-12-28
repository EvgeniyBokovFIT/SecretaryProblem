using System.Text.Json;
using DataContracts;
using HostedServiceAndDI.Configuration;
using MassTransit;

namespace Consumer;

public class PrincessClient : IConsumer<ContenderDto>
{
    private static StrategyClient? _strategy;
    
    private readonly int _contendersCount = Config.GetContendersCount();
    
    private readonly int _attemptsCount = Config.GetAttemptsCount();
    
    private HttpClient _httpClient = new();

    private JsonSerializerOptions _options;

    private static int _tryId = 1;

    private static double avg;

    public PrincessClient()
    {
        if (_strategy is null)
        {
            _strategy = new StrategyClient();
        }
        _httpClient.BaseAddress = new Uri("https://localhost:7194/hall/");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

    }

    private async Task GetNextContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/next", null);
    }

    private async Task<RatingDto> SelectContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/select", null);
        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            var ratingDto = await JsonSerializer
                .DeserializeAsync<RatingDto>(stream, _options);
            Console.WriteLine("CHOSEN RATING " + ratingDto.Rank);
            return ratingDto;
        }
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

    public Task Consume(ConsumeContext<ContenderDto> context)
    {
        var contender = context.Message;
        
        if (_strategy.IsChosenContender(contender, _tryId) || contender.Name is null)
        {
            avg += GetHappiness(SelectContender(_tryId).Result.Rank);
            if (_tryId == _attemptsCount)
            {
                Console.WriteLine(avg / Convert.ToDouble(_attemptsCount));
            }
            _tryId++;
            _strategy.Reset();
        }

        if (_tryId <= 100)
        {
            GetNextContender(_tryId);
        }
        
        return Task.CompletedTask;
    }
    
}