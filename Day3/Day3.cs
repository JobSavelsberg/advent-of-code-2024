using System.Text.RegularExpressions;

public class Day3 : IAdventOfCodeDay
{
    public void Solve()
    {
        var fileName = "./Day3/input.txt";
        var inputLines = File.ReadAllLines(fileName);
        var inputString = string.Join("", inputLines);

        var sumDo = 0;
        var sumAll = 0;
        var doLines = inputString.Split("do()").ToList();
        foreach (var line in doLines)
        {
            var doTillDont = line.Split("don't()")[0];
            sumDo += mulSum(doTillDont);
            sumAll += mulSum(line);

        }

        Console.WriteLine($"Sum of all mul: {sumAll}");
        Console.WriteLine($"Sum of all mul in do: {sumDo}");
    }

    private int mulSum(string input)
    {
        var mulRegex = new Regex(@"mul\((\d+),(\d+)\)");
        var sum = 0;
        var matches = mulRegex.Matches(input);
        matches.ToList().ForEach(match => {
            var x = int.Parse(match.Groups[1].Value);
            var y = int.Parse(match.Groups[2].Value);
            sum += x * y;
        });

        return sum;
    }
}