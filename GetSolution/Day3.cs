namespace Days;

using DaySetup;

public class Day3 : Day
{
    private int getScore(char c) {
        return c == char.ToUpper(c) ? char.ToUpper(c) - 38 : char.ToUpper(c) - 64;
    }
    private int mapLine (string line) {
        HashSet<char> part1 = line.Substring(0, line.Length / 2).ToHashSet();
        HashSet<char> part2 = line.Substring(line.Length / 2).ToHashSet();
        
        part1.IntersectWith(part2);
        return getScore(part1.First());
    }

    private int mapLines2 (string line1, string line2, string line3) {
        HashSet<char> part1 = line1.ToHashSet();
        
        part1.IntersectWith(line2.ToHashSet());
        part1.IntersectWith(line3.ToHashSet());

        return getScore(part1.First());
    }

    public override string part1(string input) {
        return input.Split("\n").Select(line => mapLine(line)).Sum().ToString();
    }

    public override string part2(string input) {
        string[] split = input.Split("\n");
        int sum = 0;

        for(int i = 0; i < split.Length; i+=3)
            sum+= mapLines2(split[i], split[i+1], split[i+2]);
        
        return sum.ToString();
    }
}
