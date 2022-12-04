namespace Days;

using DaySetup;

public class Day4 : Day
{
    private bool mapLine (string line, bool first) {
        int[][] bnds = line.Split(",").Select(
            item => item.Split("-").Select(i => int.Parse(i)).ToArray()
        ).ToArray();

        bool withinBounds = (
            bnds[0][0] <= bnds[1][0] && bnds[0][1] >= bnds[1][1]
        ) || (
            bnds[0][0] >= bnds[1][0] && bnds[0][1] <= bnds[1][1]
        );

        return first ? withinBounds : withinBounds || (
                bnds[0][0] >= bnds[1][0] && bnds[0][0] <= bnds[1][1]
            ) || (
                bnds[0][1] >= bnds[1][0] && bnds[0][1] <= bnds[1][1]
            );
    }

    public override string part1(string input) {
        return input.Split("\n").Select(line => mapLine(line, true)).Where(i => i).Count().ToString();
    }

    public override string part2(string input) {
        return input.Split("\n").Select(line => mapLine(line, false)).Where(i => i).Count().ToString();
    }
}
