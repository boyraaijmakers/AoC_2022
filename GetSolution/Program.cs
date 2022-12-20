using UtilLibrary;
using Days;
using DaySetup;

class GetSolution {

    static Day[] days = {
        new Day1(), new Day2(), new Day3(), new Day4(), new Day5(), new Day6(), 
        new Day7(), new Day8(), new Day9(), new Day10(), new Day11() , new Day12(),
        new Day13(), new Day14(), new Day15(), new Day16(), new Day17(), new Day18(),
        new Day19(), new Day20()
    };

    static void Main(string[] args) {
        DateTime start = DateTime.Now;
        
        string input = AoCUtils.readInput(args[0]);
        Day day = days[int.Parse(args[0]) - 1];

        Console.WriteLine(args[1] == "1" ? day.part1(input) : day.part2(input));
        
        TimeSpan taken = DateTime.Now - start;
        Console.WriteLine($"Finished processing in {(taken.Seconds == 0 ? taken.Milliseconds + "m" : taken.Seconds)}s");
    }
}