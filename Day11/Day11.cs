using System.Data;
using System.Numerics;

public class Day11 : IAdventOfCodeDay
{
    readonly int numberOfIterations = 75;

    // For each stone, keep track of how many stones it will become after a certain number of iterations
    readonly Dictionary<BigInteger, long[]> stoneBlinkCount = [];
    
    public void Solve()
    {
        //long[] stones  = new LinkedList<int>([0, 1, 10, 99, 999]);
        //long[] stones = [125, 17];
        long[] stones = [773, 79858, 0, 71, 213357, 2937, 1, 3998391];

        var answer  = stones.Sum(stone => CalculateNumberOfStones(stone, numberOfIterations));

        Console.WriteLine($"The total number of stones after {numberOfIterations} iterations is {answer}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stone"></param>
    /// <param name="iterationsLeft"></param>
    /// <returns>Number of stones the current stone will become after number of iterations left</returns>
    private long CalculateNumberOfStones(BigInteger stone, int iterationsLeft) {
        if(iterationsLeft == 0) {
            return 1;
        }

        if(stoneBlinkCount.ContainsKey(stone) && stoneBlinkCount[stone][iterationsLeft - 1] > 0){
                return stoneBlinkCount[stone][iterationsLeft - 1];
        }else{
            // blink
            var newIterationsLeft = iterationsLeft - 1;
            long fullStonesCount;

            if (stone == 0)
            {
                fullStonesCount = CalculateNumberOfStones(1, newIterationsLeft);
            }
            else
            {
                var stoneDigits = (long)Math.Floor(Math.Log10((double)stone)) + 1;
                if (stoneDigits % 2 == 0)
                {
                    var stoneFirstHalf = (long)((double)stone / Math.Pow(10, stoneDigits / 2));
                    var stoneSecondHalf = (long)((double)stone % Math.Pow(10, stoneDigits / 2));
                    var firstStone = CalculateNumberOfStones(stoneFirstHalf, newIterationsLeft);
                    var secondStone = CalculateNumberOfStones(stoneSecondHalf, newIterationsLeft);
                    fullStonesCount = firstStone + secondStone;
                }
                else
                {
                    fullStonesCount = CalculateNumberOfStones(stone*2024, iterationsLeft - 1);
                }
            }

            // Update memoization
            if(!stoneBlinkCount.ContainsKey(stone)){
                stoneBlinkCount[stone] = new long[numberOfIterations];
            }
            stoneBlinkCount[stone][newIterationsLeft] = fullStonesCount;
            return fullStonesCount;
        }
    }
}