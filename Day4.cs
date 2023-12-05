using Xunit;
using Xunit.Abstractions;

namespace AoC2023;

public class Day4
{
    private static IEnumerable<int> GetCardNumbers(string card)
    {
        return card.Split(' ').Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse);
    }

    private static int GetCardMatchCount(string card)
    {
        var split = card.Split('|');
        var winning = GetCardNumbers(split[0]);
        var got = GetCardNumbers(split[1]);
        return got.Count(winning.Contains);
    }

    private static int GetCardScore(string card)
    {
        var wonCount = GetCardMatchCount(card);
        return wonCount switch
        {
            0 => 0,
            1 => 1,
            _ => (int)Math.Pow(2, wonCount - 1)
        };
    }

    public static int Part1(string inputPath)
    {
        var cards = File.ReadLines(inputPath).Select(x => x.Split(':')[1]);
        return cards.Sum(GetCardScore);
    }

    public static int Part2(string inputPath)
    {
        var cards = File.ReadLines(inputPath);
        var baseMatches = new Dictionary<int, int>();
        foreach (var card in cards)
        {
            var splits = card.Split(":");
            var strId = string.Concat(splits[0].Where(char.IsDigit));
            var id = int.Parse(strId);
            var matchCount = GetCardMatchCount(splits[1]);
            baseMatches.Add(id, matchCount);
        }

        var cardCount = new Dictionary<int, int>();
        foreach (var (id, _) in baseMatches)
            cardCount.Add(id, 1);

        foreach (var (id, count) in cardCount)
        {
            var wins = baseMatches[id];
            for (var currCount = 0; currCount < count; ++currCount)
            {
                for (var i = 1; i <= wins; ++i)
                    cardCount[id + i]++;
            }
        }

        return cardCount.Values.Sum();
    }
}

public class Tests4(ITestOutputHelper logger)
{
    [Fact]
    public void Test1()
    {
        var sum = Day4.Part1("./Input/41test.txt");
        Assert.Equal(13, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve1()
    {
        var sum = Day4.Part1("./Input/4.txt");
        logger.WriteLine($"Sum is: {sum}");
    }

    [Fact]
    public void Test2()
    {
        var sum = Day4.Part2("./Input/41test.txt");
        Assert.Equal(30, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve2()
    {
        var cardCount = Day4.Part2("./Input/4.txt");
        logger.WriteLine($"Count is: {cardCount}");
    }
}