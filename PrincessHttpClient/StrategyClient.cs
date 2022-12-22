using System.Text;
using System.Text.Json;
using DataContracts;
using HostedServiceAndDI.Configuration;

namespace PrincessHttpClient;

public class StrategyClient
{
    private readonly int _contendersCount = Config.GetContendersCount();
    
    private ContenderDto _bestContender;

    private HttpClient _httpClient = new();

    public List<ContenderDto> ViewedContenders { get; set; }

    public StrategyClient()
    {
        _bestContender = new ContenderDto
        {
            Name = null
        };
        ViewedContenders = new List<ContenderDto>();
    }

    public void Reset()
    {
        _bestContender = new ContenderDto
        {
            Name = null
        };
        ViewedContenders.Clear();
    }
    
    public bool IsChosenContender(ContenderDto contender, int tryId)
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

    private bool IsChosenContenderFromFirstPart(ContenderDto contender, int tryId)
    {
        var oldBest = _bestContender;
        _bestContender = Compare(_bestContender, contender, tryId).Result;

        return ContenderIsBetterThanPrevious(30, tryId);

    }

    private async Task<ContenderDto> Compare(ContenderDto contender1, ContenderDto contender2, int tryId)
    {
        _httpClient.BaseAddress = new Uri($"https://localhost:7194/freind/{tryId}/compare");
        var compareDto = new CompareDto
        {
            Name1 = contender1.Name,
            Name2 = contender2.Name
        };
        var content = new StringContent(
            JsonSerializer.Serialize(compareDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("", content);
        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            var bestContender = await JsonSerializer
                .DeserializeAsync<ContenderDto>(stream);
            return bestContender;
        } 

    }

    private bool IsChosenContenderFromLastPart(ContenderDto contender, int tryId)
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
        
        ContenderDto contender = ViewedContenders[viewedContendersCount - 1];
        var contenderBetterThan = ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == Compare(contender, checkedContender, tryId).Result);
        
        // if (viewedContendersCount < 42)
        // {
        //     return false;
        // }

        if (contenderBetterThan < viewedContendersCount - 5 || contenderBetterThan == viewedContendersCount - 2)
        {
            return false;
        }
        
        if (contenderBetterThan is > 92 and < 95 && viewedContendersCount > 95 
                                                 && contenderBetterThan != viewedContendersCount - 4
                                                 && contenderBetterThan != viewedContendersCount - 2)
        {
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
            return true;
        }

        return false;
    }
    
    private bool IsContenderGivePoints(ContenderDto contender, int tryId)
    {
        var lastContenderBetterThan = ViewedContenders.Count(checkedContender =>
            contender != checkedContender && contender == Compare(contender, checkedContender, tryId).Result);
        var lastContenderRating = lastContenderBetterThan + 1;
        if (lastContenderRating is 100 or 98 or 96)
        {
            return true;
        }

        return false;
    }
}