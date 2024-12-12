using System.Data;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

public class Day12 : IAdventOfCodeDay
{

    private char[,] gardenPlants;
    private int[,] gardenRegionIds;
    private Dictionary<int, int> regionIdRedirect;
    private Dictionary<int, char> regionIdToPlant;
    private Dictionary<int, int> regionIdToArea;
    private Dictionary<int, int> regionIdToFenceCount;
    
    
    public Day12()
    {
                // Can we make the assumption it is square?
        var fileName = "./Day12/input.txt";
        gardenPlants = ReadGardenFromFile(fileName);
        gardenRegionIds = new int[gardenPlants.GetLength(0), gardenPlants.GetLength(1)];
        regionIdRedirect = new Dictionary<int, int>();
        regionIdToPlant = new Dictionary<int, char>();
        regionIdToArea = new Dictionary<int, int>();
        regionIdToFenceCount = new Dictionary<int, int>();

    }
    public void Solve()
    {
        for (var y = 0; y < gardenPlants.GetLength(0); y++)
        {
            for (var x = 0; x < gardenPlants.GetLength(1); x++)
            {
                gardenRegionIds[x, y] = CalculateRegionId(x, y);
            }
        }

        // Fill again garden region ids by applying the redirect (so merged region blocks are fully the same id)
        // Also fill the idToPlant dictionary
        for (var y = 0; y < gardenPlants.GetLength(0); y++)
        {
            for (var x = 0; x < gardenPlants.GetLength(1); x++)
            {
                gardenRegionIds[x, y] = GetRegionId(x, y);
                // E.g. region id 42 corresponds to plant 'E'
                regionIdToPlant[gardenRegionIds[x, y]] = gardenPlants[x, y];
            }
        }

        // Now area and fence count can be calculated per regionId
        for (var y = 0; y < gardenPlants.GetLength(0); y++)
        {
            for (var x = 0; x < gardenPlants.GetLength(1); x++)
            {
                var fences = 0;
                var currentRegionId = gardenRegionIds[x, y];

                // Check for each side for a boundary

                // Top
                if(y == 0 || gardenRegionIds[x, y-1] != currentRegionId){
                    fences++;
                }

                // Left
                if(x == 0 || gardenRegionIds[x-1, y] != currentRegionId){
                    fences++;
                }

                // Right
                if(x == gardenPlants.GetLength(0) - 1 || gardenRegionIds[x+1, y] != currentRegionId){
                    fences++;
                }

                // Bottom
                if(y == gardenPlants.GetLength(1) - 1 || gardenRegionIds[x, y+1] != currentRegionId){
                    fences++;
                }

                if (!regionIdToArea.ContainsKey(currentRegionId))
                {
                    regionIdToArea[currentRegionId] = 0;
                }

                if(!regionIdToFenceCount.ContainsKey(currentRegionId)){
                    regionIdToFenceCount[currentRegionId] = 0;
                }

                regionIdToArea[currentRegionId]++;
                regionIdToFenceCount[currentRegionId] += fences;
            }
        }

        var plantCost = new Dictionary<char, int>();

        // Now sum all regionIds to their corresponding plant
        foreach (var regionId in regionIdToPlant.Keys)
        {
            var plant = regionIdToPlant[regionId];
            var fences = regionIdToFenceCount[regionId];
            var area = regionIdToArea[regionId];

            if(!plantCost.ContainsKey(plant)){
                plantCost[plant] = 0;
            }

            plantCost[plant] += fences * area;
        }

        var totalCost = plantCost.Values.Sum();

        Console.WriteLine(totalCost);       
    }


    // Assumes we have have looked up all values for x-1 and y-1
    private int CalculateRegionId(int x, int y){
        var currentRegionId = 0;
        var currentPlant = gardenPlants[x, y];

        var sameAsLeftPlant = false;
        var sameAsUpPlant = false;

        if(x > 0){
            var leftPlant = gardenPlants[x - 1, y];
            if(currentPlant == leftPlant){
                sameAsLeftPlant = true;
            }
        }

        if(y > 0){
            var upPlant = gardenPlants[x, y - 1];
            if(currentPlant == upPlant){
                sameAsUpPlant = true;
            }
        }

        if(sameAsLeftPlant && !sameAsUpPlant){
            // Just copy left
            currentRegionId = GetRegionId(x - 1, y);
        }

        if(!sameAsLeftPlant && sameAsUpPlant){
            currentRegionId = GetRegionId(x, y - 1);
        }
        
        if(sameAsLeftPlant && sameAsUpPlant){
            var leftRegionId = GetRegionId(x - 1, y);
            var upRegionId = GetRegionId(x, y - 1);

            // Are the regionIds the same?
            if(leftRegionId == upRegionId){
                // They are the same, just return the top one
                currentRegionId = upRegionId;
            }else{
                // They need to be merged!
                // Take the upper one, and redirect the left one
                currentRegionId = upRegionId;
                regionIdRedirect[leftRegionId] = upRegionId;
            }
        }

        if(!sameAsLeftPlant && !sameAsUpPlant){
            // If not the same for up or left, it is a disconnected region
            currentRegionId = GenerateNewRegionId(x, y);
        }

        return currentRegionId;
    }

    private int GetRegionId(int x, int y)
    {
        var newRegionId = gardenRegionIds[x, y];
        while(regionIdRedirect.ContainsKey(newRegionId)){
            newRegionId = regionIdRedirect[newRegionId];
        }
        return newRegionId;
    }




    private int GenerateNewRegionId(int x, int y)
    {
        return x + y * gardenPlants.GetLength(0);
    }
    

    private static char[,] ReadGardenFromFile(string fileName){
        var input = File.ReadAllLines(fileName);

        // Input is an xSize by ySize grid of chars
        var xSize = input[0].Length;
        var ySize = input.Length;

        var garden = new char[xSize, ySize];

        for (var y = 0; y < ySize; y++)
        {
            for (var x = 0; x < xSize; x++)
            {
                garden[x, y] = input[y][x];
            }
        }

        return garden;
    }

    private static void PrintGarden(char[,] garden)
    {
        for (var y = 0; y < garden.GetLength(0); y++)
        {
            for (var x = 0; x < garden.GetLength(1); x++)
            {
                Console.Write(garden[x, y] + " ");
            }
            Console.WriteLine();
        }
    }

    private static void PrintRegionIds(int[,] regionIds)
    {
        for (var y = 0; y < regionIds.GetLength(0); y++)
        {
            for (var x = 0; x < regionIds.GetLength(1); x++)
            {
                Console.Write(regionIds[x, y].ToString().PadLeft(3) + " ");
            }
            Console.WriteLine();
        }
    }
}