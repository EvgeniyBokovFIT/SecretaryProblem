using System.Text.Json;
using DataContracts;
using HostedServiceAndDI.Configuration;
using HostedServiceAndDI.Exceptions;

namespace PrincessHttpClient;

public class PrincessClient
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
        
        for (int i = 1; i <= 1; i++)
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
        ContenderDto? chosenContender = ChooseContender(tryId);
        Console.WriteLine($"Chosen contender: {chosenContender.Name}");
        
        int happiness = GetHappiness(GetRating(tryId).Result.Rank);
        Console.WriteLine($"Happiness {happiness}");
        return happiness;
    }
    
    public ContenderDto? ChooseContender(int tryId)
    {
        try
        {
            for (int i = 0; i < _contendersCount; i++)
            {
                var contender = GetNextContender(tryId).Result;
                if (_strategy.IsChosenContender(contender, tryId))
                {
                    return contender;
                }
            }
            
        }
        catch(EmptyHallException)
        {
            return null;
        }

        return null;
    }

    private async Task<ContenderDto?> GetNextContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/next", null);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);

        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            
            var contender = await JsonSerializer
                .DeserializeAsync<ContenderDto>(stream, _options);
            Console.WriteLine($"_{contender.Name}_");
            return contender;
        } 
    }

    private async Task<RatingDto?> GetRating(int tryId)
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
}