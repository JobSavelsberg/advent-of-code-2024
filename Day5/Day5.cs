public class Day5 : IAdventOfCodeDay
{
    public void Solve(){
        var fileName = "./Day5/input.txt";
        var inputLines = File.ReadAllLines(fileName);

        // Read through all rules, create an ordering of every rule

        // Rules are in format "a|b", store these as tuples
        var rules = new List<(int, int)>();
        // loop over input until we reach an empty line
        rules = inputLines
            .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => (int.Parse(line.Split("|")[0]), int.Parse(line.Split("|")[1])))
            .ToList();
            

        var updates = new List<List<int>>();
        // store the next lines in the updates, they are stored as "a,b,c,d,e,..."
        updates = inputLines
            .Skip(rules.Count + 1)
            .Select(line => line.Split(",").Select(int.Parse).ToList())
            .ToList();

        var sortedRules = new LinkedList<int>();


        var updateWasChanged = updates.Select(update =>
        {
            var wasChangedToMakeValid = false;
            // Store relevant rules in a list
            var relevantRules = rules.Where(rule => update.Contains(rule.Item1) &&  update.Contains(rule.Item2)).ToList();
            
            int i = 0;
            while (i < update.Count)
            {
                var number = update[i];
                // Check if the number is in the second part of the rule, if so, something's wrong
                if (relevantRules.Any(rule => rule.Item2 == number))
                {
                    // Put number at the end of the list
                    update.RemoveAt(i);
                    update.Add(number);
                    wasChangedToMakeValid = true;
                }
                else
                {
                    // Good, now remove all rules where the number is in the first part
                    relevantRules.RemoveAll(rule => rule.Item1 == number);
                    i++;
                }
            }

            return wasChangedToMakeValid;                
        });

        // Sum all middle numbers of each valid update
        var sum = updateWasChanged.Zip(updates, (wasChanged, update) => (wasChanged, update)).Select(t =>{
            // Invert this for solution to part 1
            if(t.wasChanged){
                var middleNumber = t.update[(int)(t.update.Count /2.0)];
                return middleNumber;
            }
            return 0;
        }).Sum();

        Console.WriteLine(sum);
    }
} 