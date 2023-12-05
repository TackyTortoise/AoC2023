using Xunit;
using Xunit.Abstractions;

namespace AoC2023;

public class Day3
{
    private static IEnumerable<int> GetNeighbours(int i, string grid, int width)
    {
        var neighboursIndices = new List<int>();
        if (i % width > 0)
        {
            // Left
            neighboursIndices.Add(i - 1);
            if (i > width) // Top left
                neighboursIndices.Add(i - width - 1);
            if (i < grid.Length - width - 1) // Bottom left
                neighboursIndices.Add(i + width - 1);
        }

        if (i % width < width - 1)
        {
            // Right
            neighboursIndices.Add(i + 1);
            if (i > width) // Top right
                neighboursIndices.Add(i - width + 1);
            if (i < grid.Length - width - 1) // Bottom right
                neighboursIndices.Add(i + width + 1);
        }

        if (i > width)
        {
            neighboursIndices.Add(i - width);
        }

        if (i < grid.Length - width - 1)
        {
            neighboursIndices.Add(i + width);
        }

        return neighboursIndices;
    }

    private static (int num, IEnumerable<int> usedIndices) FindNumber(int i,
        string grid, int width)
    {
        if (!char.IsDigit(grid[i]))
            return (0, Enumerable.Empty<int>());
        var row = i / width;
        while (i % width > 0 && char.IsDigit(grid[i - 1]))
            --i;
        var num = "";
        var usedIndices = new List<int>();
        while (i % width < width && char.IsDigit(grid[i]))
        {
            num += grid[i];
            usedIndices.Add(i);
            ++i;
            if (i / width != row)
                break;
        }

        return (int.Parse(num), usedIndices);
    }

    public static int Part1(string inputPath)
    {
        var sum = 0;
        var lines = File.ReadLines(inputPath).ToList();
        var width = lines.First().Length;
        var grid = string.Join(string.Empty, lines);
        var countedIndices = new List<int>();
        for (int i = 0; i < grid.Length; ++i)
        {
            var current = grid[i];
            if (!char.IsDigit(current) && current != '.')
            {
                var neighboursIndices = GetNeighbours(i, grid, width);
                foreach (var index in neighboursIndices)
                {
                    if (!countedIndices.Contains(index) &&
                        char.IsDigit(grid[index]))
                    {
                        var (num, usedIndices) = FindNumber(index, grid, width);
                        sum += num;
                        countedIndices.AddRange(usedIndices);
                    }
                }
            }
        }

        return sum;
    }

    public static int Part2(string inputPath)
    {
        var sum = 0;
        var lines = File.ReadLines(inputPath).ToList();
        var width = lines.First().Length;
        var grid = string.Join(string.Empty, lines);
        var countedIndices = new List<int>();
        var gearIndex = 0;
        while (gearIndex >= 0)
        {
            gearIndex = grid.IndexOf('*', gearIndex + 1);
            var neighbourIndices = GetNeighbours(gearIndex, grid, width);
            var values = new List<int>();
            foreach (var index in neighbourIndices)
            {
                if (!countedIndices.Contains(index) &&
                    char.IsDigit(grid[index]))
                {
                    var (num, usedIndices) = FindNumber(index, grid, width);
                    values.Add(num);
                    countedIndices.AddRange(usedIndices);
                }
            }

            if (values.Count == 2)
                sum += values.Aggregate((x, y) => x * y);
        }

        return sum;
    }
}

public class Tests3(ITestOutputHelper logger)
{
    [Fact]
    public void Test1()
    {
        var sum = Day3.Part1("./Input/31test.txt");
        Assert.Equal(4361, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve1()
    {
        var sum = Day3.Part1("./Input/3.txt");
        logger.WriteLine($"Sum is: {sum}");
    }

    [Fact]
    public void Test2()
    {
        var sum = Day3.Part2("./Input/31test.txt");
        Assert.Equal(467835, sum);
    }

    [Fact]
    [Trait("Category", "Solve")]
    public void Solve2()
    {
        var sum = Day3.Part2("./Input/3.txt");
        logger.WriteLine($"Sum is: {sum}");
    }
}