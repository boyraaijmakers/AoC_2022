namespace Days;

using DaySetup;

public class Day10 : Day
{
    
    public override string part1(string input) {
        string[] inputLines = input.Split("\n");
        int clock = 0;
        int x = 1;
        int sum = 0;

        foreach(string line in inputLines) {
            if (line == "noop") {
                clock++;
                if ((clock - 20) % 40 == 0 ) sum += clock * x;
            }
            else {
                clock += 2;
                if ((clock - 20) % 40 == 0 ) sum += clock * x;
                if ((clock - 20) % 40 == 1 ) sum += (clock - 1) * x;
                x += int.Parse(line.Split(" ")[1]);
            }
        }

        return sum.ToString();
    }
    
    public override string part2(string input) {
        string[] inputLines = input.Split("\n");
        bool[,] grid = new bool[6, 40];
        int clock = 0;
        int x = 1;

        foreach(string line in inputLines) {
            if (line == "noop") {
                if(Math.Abs(x - (clock % 40)) < 2) 
                    grid[(clock - clock % 40)/40, clock % 40] = true;
                clock++;
            }
            else {
                foreach(int _ in Enumerable.Repeat(0, 2)){
                    if(Math.Abs(x - (clock % 40)) < 2) 
                        grid[(clock - clock % 40)/40, clock % 40] = true;
                    clock++;
                }
                x += int.Parse(line.Split(" ")[1]);
            }
        }
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 40; j++) 
               Console.Write(grid[i,j] ? "#": " ");
            Console.WriteLine();
         }
        return ""; 
    }
}