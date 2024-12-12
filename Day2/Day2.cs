public class Day2 : IAdventOfCodeDay
{
    public void Solve()
    {
        var fileName = "./Day2/input.txt";
        var input = File.ReadAllLines(fileName);

        var reports = input.Select(x => x.Split(" ").Select(int.Parse).ToList()).ToList();

        var safeReportCount = reports.Where(report => IsSafe(report, true)).Count();
        var safeReportCountWithDampener = reports.Where(report => IsSafe(report)).Count();

        Console.WriteLine($"Safe reports: {safeReportCount}");
        Console.WriteLine($"Safe reports with dampener: {safeReportCountWithDampener}");
    }

    public bool IsSafe(List<int> report, bool usedDampener = false)
    {
        // Since one of these diffs might not be valid, we need to check at least 3
        var increasing = new double[] { Math.Sign(report[1] - report[0]), Math.Sign(report[2] - report[1]), Math.Sign(report[3] - report[2]) }.Average() > 0;

        for (int i = 0; i < report.Count - 1; i++)
        {
            var incorrectDirection = increasing && report[i] > report[i + 1] || !increasing && report[i] < report[i + 1];
            
            var difference = Math.Abs(report[i] - report[i + 1]);
            var incorrectDifference = difference < 1 || difference > 3;

            if (incorrectDirection || incorrectDifference)
            {
                if(usedDampener){
                    return false;
                }else{
                    // Check by removing both i and i + 1
                    var newReport1 = report.Take(i).Concat(report.Skip(i + 1)).ToList();
                    var newReport2 = report.Take(i + 1).Concat(report.Skip(i + 2)).ToList();

                    return IsSafe(newReport1, true) || IsSafe(newReport2, true);
                }
            }
        }

        return true;
    }
}