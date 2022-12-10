namespace Days;

using DaySetup;

public class Day9 : Day
{
    private int getTailSpaceCount(string input, int knotCount) {
        string[] inputLines = input.Split("\n");

        Dictionary<string, (int x, int y)> directions = new Dictionary<string, (int, int)>()
            {{"D", (0, -1)}, {"U", (0, 1)}, {"L", (-1, 0)}, {"R", (1, 0)}};

        (int x, int y)[] knots = Enumerable.Repeat((0, 0), knotCount).ToArray();
        HashSet<string> visited = new HashSet<string>() {"0,0"};

        foreach(string line in inputLines) {
            string[] lineParts = line.Split(" ");
            string direction = lineParts[0];
            int movesTodo = int.Parse(lineParts[1]);

            foreach(var _ in Enumerable.Range(0, movesTodo)) {
                knots[0].x += directions[direction].x;
                knots[0].y += directions[direction].y;

                for(int i = 0; i < knotCount - 1; i++) {
                    if(Math.Abs(knots[i].x - knots[i+1].x) > 1) {
                        knots[i+1].x = knots[i].x > knots[i+1].x ? knots[i].x - 1 : knots[i].x + 1;

                        if (Math.Abs(knots[i].y - knots[i+1].y) > 1) 
                            knots[i+1].y = knots[i].y > knots[i+1].y ? knots[i].y - 1 : knots[i].y + 1;
                        else if (knots[i].y != knots[i+1].y) 
                            knots[i+1].y = knots[i].y;
                    }
                    else if(Math.Abs(knots[i].y - knots[i+1].y) > 1) {
                        knots[i+1].y = knots[i].y > knots[i+1].y ? knots[i].y - 1 : knots[i].y + 1;

                        if (Math.Abs(knots[i].x - knots[i+1].x) > 1) 
                            knots[i+1].x = knots[i].x > knots[i+1].x ? knots[i].x - 1 : knots[i].x + 1;
                        else if (knots[i].x != knots[i+1].x) 
                            knots[i+1].x = knots[i].x;
                    }
                }

                visited.Add(knots[knotCount - 1].x + "," + knots[knotCount - 1].y);
            }
        } 

        return visited.Count;
    }
    public override string part1(string input) {
        return getTailSpaceCount(input, 2).ToString();
    }

    public override string part2(string input) {
        return getTailSpaceCount(input, 10).ToString();  
    }
}
