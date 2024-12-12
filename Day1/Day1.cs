using System.Reflection;

public class Day1 : IAdventOfCodeDay
{
    public void Solve()
    {
        var fileName = "./Day1/input.txt";
        var input = File.ReadAllLines(fileName);

        // Parse lines to two integer arrays
        var leftNumber = input.Select(x => x.Split("   ")[0]).Select(int.Parse).ToArray();
        var rightNumber = input.Select(x => x.Split("   ")[1]).Select(int.Parse).ToArray();

        // Sort both by smallest
        Array.Sort(leftNumber);
        Array.Sort(rightNumber);

        // Calculate the distance of each pair and sum this
        var sum = leftNumber.Zip(rightNumber, (l, r) => Math.Abs(l - r)).Sum();

        Console.WriteLine($"The sum of the distances is {sum}");
    }
}