using System.Text.Json;
using DataContracts;
using HostedServiceAndDI.Configuration;
using Microsoft.Extensions.Hosting;

namespace PrincessHttpClient;

public class PrincessClient : IHostedService
{
    private StrategyClient _strategy;
    
    private readonly int _contendersCount = Config.GetContendersCount();
    
    private readonly int _attemptsCount = Config.GetAttemptsCount();
    
    private HttpClient _httpClient = new();

    private JsonSerializerOptions _options;

    public PrincessClient(StrategyClient strategy)
    {
        _strategy = strategy;
        _httpClient.BaseAddress = new Uri("https://localhost:7194/hall/");
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public double DoSeveralTries()
    {
        double avg = 0;
        
        for (int i = 1; i <= _attemptsCount; i++)
        {
            _strategy.Reset();
            avg += SimulatePrincessBehaviourOnCurrentTry(i);
        }

        avg /= _attemptsCount;
        Console.WriteLine($"Average happiness = {avg}");
        return avg;
    }
    
    private int SimulatePrincessBehaviourOnCurrentTry(int tryId)
    {
        ContenderDto chosenContender = ChooseContender(tryId);

        Console.WriteLine($"Chosen contender: {chosenContender.Name}");
        
        int happiness = GetHappiness(GetRating(tryId).Result.Rank);
        Console.WriteLine($"Happiness {happiness}");
        return happiness;
    }
    
    public ContenderDto ChooseContender(int tryId)
    {

        for (int i = 1; i <= _contendersCount + 1; i++)
        {
            var contender = GetNextContender(tryId).Result;
            if (_strategy.IsChosenContender(contender, tryId))
            {
                return contender;
            }
        }
        
        return new ContenderDto
        {
            Name = null
        };
    }

    private async Task<ContenderDto?> GetNextContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/next", null);

        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            var contender = await JsonSerializer
                .DeserializeAsync<ContenderDto>(stream, _options);
            
            return contender;
        } 
    }

    private async Task<RatingDto> GetRating(int tryId)
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
        _strategy.Reset();
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(DoSeveralTries);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}