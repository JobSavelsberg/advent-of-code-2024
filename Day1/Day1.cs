using System.Reflection;

public class Day1 : IAdventOfCodeDay
{
    public void Solve()
    {
        var fileName = "./Day1/input.txt";
        var input = File.ReadAllLines(fileName);

        // SolvePart1(input);
        SolvePart2(input);
    }

    public void SolvePart1(string[] input){
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

    public void SolvePart2(string[] input){
        // Create two lists of numbers
        var leftNumber = input.Select(x => x.Split("   ")[0]).Select(int.Parse);
        var rightNumber = input.Select(x => x.Split("   ")[1]).Select(int.Parse);

        var numberOcurranceLeft = leftNumber.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var numberOcurranceRight = rightNumber.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        // For each number in the left list, multiply it by the left occurance and right occurance
        // Sum all the results, default to 0 if not exists in the right list
        var sum = leftNumber.Select(x => x * numberOcurranceLeft[x] * numberOcurranceRight.GetValueOrDefault(x, 0)).Sum();

        Console.WriteLine($"Similarity score is {sum}");
    }
}