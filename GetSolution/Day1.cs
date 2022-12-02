namespace Days;

using DaySetup;

public class Day1 : Day
{
    private int[] getSums(string input) {
        return input.Split("\n\n").Select(
            item => item.Split("\n").Select(
                item => int.Parse(item)
            ).Aggregate(
                0, (acc, x) => acc + x
            )
        ).ToArray();
    }

    public override string part1(string input) {
        return getSums(input).Max().ToString();
    }

    public override string part2(string input) {
        return getSums(input).OrderByDescending(i => i).Take(3).Sum().ToString();
    }
}
