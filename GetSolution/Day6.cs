namespace Days;

using DaySetup;

public class Day6 : Day
{

    private int getDistinct(string input, int horizon) {
        for (int i = 0; i < input.Length - horizon; i++) 
            if (input.ToCharArray().Skip(i).Take(horizon).ToHashSet().Count == horizon)
                return i+horizon;

        return -1;
    }

    public override string part1(string input) {
        return getDistinct(input, 4).ToString();
    }

    public override string part2(string input) {
        return getDistinct(input, 14).ToString();
    }
}
