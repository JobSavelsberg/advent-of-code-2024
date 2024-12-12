
// Find all days implementing IAdcentOfCodeDay
using System.Reflection;

var dayTypes = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.GetInterfaces().Contains(typeof(IAdventOfCodeDay)));

var dayNumber = 12;

// Find the class with the day number
var dayType = dayTypes.FirstOrDefault(t => t.Name.Contains($"Day{dayNumber}"));

if(dayType == null)
{
    Console.WriteLine($"Day {dayNumber} not found");
    return;
}

// Instantiate the class and call Solve
var day = Activator.CreateInstance(dayType) as IAdventOfCodeDay;
day.Solve();
