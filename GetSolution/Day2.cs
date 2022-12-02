namespace Days;

using DaySetup;

public class Day2 : Day
{
    private int mapLine (string line) {
        return line switch {
            "A X" => 4, "A Y" => 8, "A Z" => 3,
            "B X" => 1, "B Y" => 5, "B Z" => 9,
            "C X" => 7, "C Y" => 2, "C Z" => 6,
            _ => 0
        };
    }

    private int mapLine2 (string line) {
        return line switch {
            "A X" => 3, "A Y" => 4, "A Z" => 8,
            "B X" => 1, "B Y" => 5, "B Z" => 9,
            "C X" => 2, "C Y" => 6, "C Z" => 7,
            _ => 0
        };
    }

    public override string part1(string input) {
        return input.Split("\n").Select(line => mapLine(line)).Sum().ToString();
    }

    public override string part2(string input) {
        return input.Split("\n").Select(line => mapLine2(line)).Sum().ToString();
    }
}
