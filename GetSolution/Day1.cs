namespace Days;

using UtilLibrary;
using DaySetup;

public class Day1 : Day
{
    private int[] getSums(string input) {
        return AoCUtils.tokenize(input, "\n\n").Select(
            item => AoCUtils.tokenize(item, "\n").Select(
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
