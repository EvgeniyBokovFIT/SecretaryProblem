using System.Text;
using System.Text.Json;
using Nsu.PeakyBride.DataContracts;

namespace Consumer;

public class StrategyClient
{
    private readonly int _contendersCount = 100;

    private List<Contender> _fivePrevious = new List<Contender>();

    private static Contender _bestContender = new Contender
    {
        Name = null
    };

    private HttpClient _httpClient = new();

    public static List<Contender> ViewedContenders { get; set; } = new List<Contender>();

    public StrategyClient()
    {
        Console.WriteLine("STRAT CONST");
        
        _httpClient.BaseAddress = new Uri("https://nsupeakybrideapi20221215134314.azurewebsites.net/api/freind/");

    }

    public void Reset()
    {
        _bestContender = new Contender
        {
            Name = null
        };
        ViewedContenders.Clear();
    }
    
    
    public bool IsChosenContenderFastStrategy(Contender contender, int tryId)
    {
        if (_fivePrevious.Count < 5)
        {
            _fivePrevious.Add(contender);
            return false;
        }

        bool betterThanAll = true;
        foreach (var prevContender in _fivePrevious)
        {
            if (!contender.Name.Equals(Compare(contender, prevContender, tryId).Result.Name))
            {
                betterThanAll = false;
                break;
            }
        }

        if (betterThanAll)
        {
            return true;
        }

        for (int i = 0; i < 4; i++)
        {
            _fivePrevious[i] = _fivePrevious[i + 1];
        }

        _fivePrevious[4] = contender;
        return false;
    }
    
    public bool IsChosenContender(Contender contender, int tryId)
    {
        ViewedContenders.Add(contender);
        if (ViewedContenders.Count == 1)
        {
            _bestContender = contender;
            return false;
        }
        if (ViewedContenders.Count < _contendersCount / 2)
        {
            return IsChosenContenderFromFirstPart(contender, tryId);
        }

        return IsChosenContenderFromLastPart(contender, tryId);
    }

    private bool IsChosenContenderFromFirstPart(Contender contender, int tryId)
    {
        var oldBest = _bestContender;
        _bestContender = Compare(_bestContender, contender, tryId).Result;

        return ContenderIsBetterThanPrevious(30, tryId);

    }

    private async Task<Contender> Compare(Contender contender1, Contender contender2, int tryId)
    {
        var compareDto = new CompareDto
        {
            Name1 = contender1.Name,
            Name2 = contender2.Name
        };
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var content = new StringContent(
            JsonSerializer.Serialize(compareDto, options), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{tryId}/compare?session=7re-qnd-5pu-hld", content);
        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            
            var bestContender = await JsonSerializer
                .DeserializeAsync<Contender>(stream, options);
            return bestContender;
        } 

    }

    private bool IsChosenContenderFromLastPart(Contender contender, int tryId)
    {
        if (ViewedContenders.Count == _contendersCount)
        {
            if (IsContenderGivePoints(contender, tryId))
            {
                return true;
            }

            return false;
        }

        _bestContender = Compare(_bestContender, contender, tryId).Result;
        
        return ContenderIsBetterThanPrevious(Convert.ToInt32(ViewedContenders.Count * 0.95), tryId);
    }

    private bool ContenderIsBetterThanPrevious(int numOfContenders, int tryId)
    {
        int viewedContendersCount = ViewedContenders.Count;
        
        if (viewedContendersCount < 50)
        {
            return false;
        }
        
        Contender contender = ViewedContenders[viewedContendersCount - 1];
        var contenderBetterThan = ViewedContenders.Count(checkedContender =>
            contender.Name == Compare(contender, checkedContender, tryId).Result.Name) - 1;
        
        

        if (contenderBetterThan < viewedContendersCount - 5 || contenderBetterThan == viewedContendersCount - 2)
        {
            return false;
        }
        
        if (contenderBetterThan is > 92 and < 95 && viewedContendersCount > 95 
                                                 && contenderBetterThan != viewedContendersCount - 4
                                                 && contenderBetterThan != viewedContendersCount - 2)
        {
            Console.WriteLine("1");
            return true;
        }

        if (viewedContendersCount < 60 && contenderBetterThan < viewedContendersCount - 2)
        {
            return false;
        }
        
        
        if (viewedContendersCount < 80 && (contenderBetterThan < viewedContendersCount - 3 
                                           || contenderBetterThan == viewedContendersCount - 1))
        {
            return false;
        }

        if (viewedContendersCount >= 95 && contenderBetterThan == viewedContendersCount - 4)
        {
            return false;
        }

        if (contenderBetterThan >= numOfContenders)
        {
            Console.WriteLine("CONT BETTER THAN " + contenderBetterThan);
            Console.WriteLine("NUM OF CONT " + numOfContenders);
            Console.WriteLine("2");
            return true;
        }

        return false;
    }
    
    private bool IsContenderGivePoints(Contender contender, int tryId)
    {
        var lastContenderBetterThan = ViewedContenders.Count(checkedContender =>
            contender.Name == Compare(contender, checkedContender, tryId).Result.Name);
        var lastContenderRating = lastContenderBetterThan;
        if (lastContenderRating is 100 or 98 or 96)
        {
            return true;
        }

        return false;
    }
}