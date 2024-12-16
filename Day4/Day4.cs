using System.Drawing;
using System.Text.RegularExpressions;

public class Day4 : IAdventOfCodeDay
{
    private string searchString = "XMAS";

    private readonly Dictionary<string, (int dx, int dy)> windDirections = new Dictionary<string, (int dx, int dy)>
    {
        { "N", (0, -1) },
        { "S", (0, 1) },
        { "E", (1, 0) },
        { "W", (-1, 0) },
        { "NE", (1, -1) },
        { "NW", (-1, -1) },
        { "SE", (1, 1) },
        { "SW", (-1, 1) }
    };

    private readonly Dictionary<string, Tuple<Point, Point>> xDirections = new Dictionary<string, Tuple<Point, Point>>
        {
            { "N", new Tuple<Point, Point>(new Point(-1, -1), new Point(1, -1)) },
            { "S", new Tuple<Point, Point>(new Point(-1, 1), new Point(1, 1)) },
            { "E", new Tuple<Point, Point>(new Point(1, -1), new Point(1, 1)) },
            { "W", new Tuple<Point, Point>(new Point(-1, -1), new Point(-1, 1)) },
        };

    private char[,] input;

    public void Solve()
    {
        var fileName = "./Day4/input.txt";
        var inputLines = File.ReadAllLines(fileName);

        // Turn into char matrix
        input = new char[inputLines.Length, inputLines[0].Length];
        for (int y = 0; y < inputLines.Length; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                input[x, y] = inputLines[y][x];
            }
        }

        FindXMAS();
        FindX_MAS();
    } 

    private void FindX_MAS(){
        var occurences = 0;
        for(int x = 1; x < input.GetLength(0)-1; x++){
            for(int y = 1; y < input.GetLength(1)-1; y++){
                // Check if there's an A
                var currentChar = input[x, y];
                if (currentChar == 'A')
                {
                    foreach (var direction in xDirections)
                    {
                        var mCandidate1 = input[x + direction.Value.Item1.X, y + direction.Value.Item1.Y];
                        if(mCandidate1 != 'M')continue;
                        
                        var mCandidate2 = input[x + direction.Value.Item2.X, y + direction.Value.Item2.Y];
                        if(mCandidate2 != 'M')continue;
                        

                        var sCandidate1 = input[x - direction.Value.Item1.X, y - direction.Value.Item1.Y ];
                        if(sCandidate1 != 'S')continue;
                        

                        var sCandidate2 = input[x - direction.Value.Item2.X, y - direction.Value.Item2.Y ];
                        if(sCandidate2 != 'S')continue;

                        occurences++;
                    }
                }
            }
        }

        Console.WriteLine($"Found {occurences} occurences of X-MAS");
    }

    private void FindXMAS(){
        var sumFound = 0;

        for(int x = 0; x < input.GetLength(0); x++){
            for(int y = 0; y < input.GetLength(1); y++){
                
                var currentChar = input[x, y];
                if (currentChar == searchString[0])
                {
                    foreach (var direction in windDirections)
                    {
                        if (FoundSearchString(x, y, searchString, direction.Key))
                        {
                            sumFound++;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Found {sumFound} occurences of {searchString}");
    }

    private bool FoundSearchString(int x, int y, string searchString, string direction){
        var dx = windDirections[direction].dx;
        var dy = windDirections[direction].dy;

        for(int i = 1; i < searchString.Length; i++){
            var currentX = x + dx * i;
            var currentY = y + dy * i;

            if(currentX < 0 || currentX >= input.GetLength(0) || currentY < 0 || currentY >= input.GetLength(1)){
                return false;
            }

            if(input[currentX, currentY] != searchString[i]){
                return false;
            }
        }

        return true;
    }
} 