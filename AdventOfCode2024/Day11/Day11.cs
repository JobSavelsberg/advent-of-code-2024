public class Day11 : IAdventOfCodeDay
{
    LinkedList<long> stones = new();
    readonly int numberOfIterations = 25;
    
    public void Solve()
    {
        // stones = new LinkedList<int>([0, 1, 10, 99, 999]);
        // stones = new LinkedList<int>([125, 17]);
        stones = new LinkedList<long>([773, 79858, 0, 71, 213357, 2937, 1, 3998391]);

        List<LinkedListNode<long>> stoneNodes = [];
        for(var stone = stones.First; stone != null; stone = stone.Next) {
            stoneNodes.Add(stone);
        }

        foreach(var stone in stoneNodes) {
            Blink(stone, numberOfIterations);
        }
        
        // Print the stones
        // foreach (var stone in stones) {
        //     Console.WriteLine(stone);
        // }
        Console.WriteLine(stones.Count);
    }

    private void Blink(LinkedListNode<long> stone, int iterationsLeft) {
        if(iterationsLeft == 0) {
            return;
        }else{
            iterationsLeft--;
        }

        if (stone.Value == 0) {
            stone.Value = 1;
            Blink(stone, iterationsLeft);
            return;
        }

        var stoneString = stone.Value.ToString();
        if(stoneString.Length % 2 == 0) {
            stone.Value = long.Parse(stoneString[..(stoneString.Length / 2)]);
            var newStone = new LinkedListNode<long>(long.Parse(stoneString[(stoneString.Length / 2)..]));
            // insert into linked list after stone
            stones.AddAfter(stone, newStone);
            Blink(stone, iterationsLeft);
            Blink(newStone, iterationsLeft);
            return;
        }

        stone.Value *= 2024;
        Blink(stone, iterationsLeft);
        return;
    }
}