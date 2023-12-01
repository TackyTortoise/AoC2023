using Xunit;
using Xunit.Abstractions;

namespace AoC2023;

public class Day1
{
    public static int Part1(string inputPath)
    {
        return File.ReadLines(inputPath).Sum(line =>
        {
            var first = line.First(char.IsDigit);
            var last = line.Last(char.IsDigit);
            var digitStr = $"{first}{last}";
            return int.Parse(digitStr);
        });
    }

    private static (int firstIndex, char firstValue, int lastIndex, char
        lastValue
        ) FindFirstDigit(string line)
    {
        var firstValue = line.FirstOrDefault(char.IsDigit);
        var lastValue = line.LastOrDefault(char.IsDigit);
        var firstIndex = firstValue == default
            ? int.MaxValue
            : line.IndexOf(firstValue);
        var lastIndex = lastValue == default
            ? int.MinValue
            : line.LastIndexOf(lastValue);
        return (firstIndex, firstValue, lastIndex, lastValue);
    }

    private static (int firstIndex, char firstValue, int lastIndex, char
        lastValue
        ) FindFirstStrDigit(string line)
    {
        var result = (int.MaxValue, default(char), int.MinValue, default(char));
        for (var i = 0; i < Nums.Length; i++)
        {
            var num = Nums[i];
            var firstIndex = line.IndexOf(num, StringComparison.Ordinal);
            var lastIndex = line.LastIndexOf(num, StringComparison.Ordinal);
            var value = (char)((char)(i + 1) + '0');
            if (firstIndex >= 0 && firstIndex < result.Item1)
            {
                result.Item1 = firstIndex;
                result.Item2 = value;
            }

            if (lastIndex >= 0 && lastIndex > result.Item3)
            {
                result.Item3 = lastIndex;
                result.Item4 = value;
            }
        }

        return result;
    }

    private static readonly string[] Nums =
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight",
        "nine"
    };

    public static int Part2(string inputPath)
    {
        return File.ReadLines(inputPath).ToList().Sum(line =>
        {
            var dig =
                FindFirstDigit(line);
            var str = FindFirstStrDigit(line);
            var firstValue = dig.firstIndex < str.firstIndex
                ? dig.firstValue
                : str.firstValue;
            var lastValue = dig.lastIndex > str.lastIndex
                ? dig.lastValue
                : str.lastValue;

            var digitStr = $"{firstValue}{lastValue}";
            return int.Parse(digitStr);
        });
    }
}

public class Tests(ITestOutputHelper logger)
{
    [Fact]
    public void Test1()
    {
        var sum = Day1.Part1("./Input/11test.txt");
        Assert.Equal(142, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve1()
    {
        var sum = Day1.Part1("./Input/1.txt");
        logger.WriteLine($"Solution part 1: {sum}");
        Assert.Equal(54450, sum);
    }

    [Fact]
    public void Test2()
    {
        var sum = Day1.Part2("./Input/12test.txt");
        logger.WriteLine($"Test 2 result: {sum}");
        Assert.Equal(281, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve2()
    {
        var sum = Day1.Part2("./Input/1.txt");
        logger.WriteLine($"Solution part 2: {sum}");
        Assert.Equal(54265, sum);
    }
}