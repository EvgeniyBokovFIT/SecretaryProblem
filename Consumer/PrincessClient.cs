﻿using System.Text.Json;
using DataContracts;
using MassTransit;

namespace Consumer;

public class PrincessClient : IConsumer<ContenderDto>
{
    private static StrategyClient? _strategy;
    
    private readonly int _contendersCount = 100;
    
    private readonly int _attemptsCount = 100;
    
    private HttpClient _httpClient = new();

    private JsonSerializerOptions _options;

    private static int _tryId = 1;

    private static int avg = 0;

    private static int i = 0;

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

        //Reset();
        //GetNextContender(1);
    }

    private async Task GetNextContender(int tryId)
    {
        var response = await _httpClient.PostAsync($"{tryId}/next", null);
        //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        
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
        i++;
        Console.WriteLine(contender.Name + " " + _tryId);
        if (_strategy.IsChosenContender(contender, _tryId))
        {
            //Console.WriteLine("FROM CHOOSE CONT");
            //Console.WriteLine(contender.Name);
            avg += GetHappiness(SelectContender(_tryId).Result.Rank);
            if (_tryId == 100)
            {
                Console.WriteLine(avg/100.0);
            }
            _tryId++;
        }

        if (_tryId <= 100)
        {
            GetNextContender(_tryId);
        }
        
        return Task.CompletedTask;
    }
    
}