namespace SecretaryProblem;

public class ContenderGenerator
{
    private readonly int _contendersSize = 100;
    private readonly Random _random = new();

    private List<string> GenerateNames()
    {
        string[] firstNames =
        {
            "Иван", "Федор", "Михаил", "Петр", "Евгений", "Александр", "Илья", "Андрей", "Дмитрий", "Амадей", "Роман",
            "Никита", "Егор", "Марк"
        };
        string[] patronymics =
        {
            "Иванович", "Георгиевич", "Петрович", "Алексеевич", "Александрович", "Евгеньевич", "Сергеевич",
            "Николаевич", "Юрьевич", "Денисович", "Андреевич", "Дмитриевич", "Михайлович", "Ильич"
        };
        var names = new List<string>();

        for (var i = 0; i < _contendersSize;)
        {
            var name = firstNames[_random.Next(firstNames.Length)] + " " +
                       patronymics[_random.Next(patronymics.Length)];
            if (!names.Contains(name))
            {
                names.Add(name);
                i++;
            }
        }

        return names;
    }

    public List<Contender> GenerateContenders()
    {
        var contenders = new List<Contender>();
        var ratings = new List<int>();
        var names = GenerateNames();

        for (var i = 0; i < _contendersSize; i++) 
            ratings.Add(i + 1);

        for (var i = 0; i < _contendersSize; i++)
        {
            var ratingsCurIndex = _random.Next(ratings.Count);
            var rating = ratings[ratingsCurIndex];
            ratings.RemoveAt(ratingsCurIndex);
            contenders.Add(new Contender(names[i], rating));
        }

        return contenders;
    }
}