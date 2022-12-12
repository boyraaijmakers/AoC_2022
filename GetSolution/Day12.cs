namespace Days;

using DaySetup;
using UtilLibrary;

public class Day12 : Day
{
    int[][] grid;
    (int x, int y) start;
    (int x, int y) end;
    
    private int findPath((int x, int y, int previousLevel, int steps) start) {
        Queue<(int x, int y, int previousLevel, int steps)> items;
        items = new Queue<(int x, int y, int currentLevel, int steps)>();
        items.Enqueue(start);

        Dictionary<(int x, int y), int> paths = new Dictionary<(int x, int y), int>();

        while(items.Count > 0) {
            (int x, int y, int previousLevel, int steps) next = items.Dequeue();
            if(paths.ContainsKey((next.x, next.y))) if(paths[(next.x, next.y)] <= next.steps) continue;

            int currentLevel = this.grid[next.x][next.y];
            if(currentLevel > next.previousLevel + 1) continue;
            if (next.y > 0 && currentLevel == 1) continue;

            paths[(next.x, next.y)] = next.steps;

            if(next.x < grid.Length - 1) items.Enqueue((next.x + 1, next.y, currentLevel, next.steps + 1));
            if(next.y < grid[0].Length - 1) items.Enqueue((next.x, next.y + 1, currentLevel, next.steps + 1));
            if(next.y > 0) items.Enqueue((next.x, next.y - 1, currentLevel, next.steps + 1));
            if(next.x > 0) items.Enqueue((next.x - 1, next.y, currentLevel, next.steps + 1));
        }

        if (paths.ContainsKey(this.end)) return paths[this.end];
        return grid.Length * grid[0].Length;
    }

    private void parseGrid(string input) {
        this.grid = input.Split("\n").Select(
            line => line.ToCharArray().Select(c => (int)c).ToArray()
        ).ToArray();

        for(int i = 0; i < this.grid.Length; i++) {
            for(int j = 0; j < this.grid[0].Length; j++) {
                if (this.grid[i][j] == 83) {
                    this.start = (i, j);
                    this.grid[i][j] = 1;
                }
                if (this.grid[i][j] == 69) {
                    this.end = (i, j);
                    this.grid[i][j] = 26;
                }

                this.grid[i][j] %= 32;
            }
        }
    }

    public override string part1(string input) {
        parseGrid(input);
        return findPath((this.start.x, this.start.y, 1, 0)).ToString();
    }
    
    public override string part2(string input) {
        parseGrid(input);

        int max = grid.Length * grid[0].Length;
        for(int i = 0; i < this.grid.Length; i++) 
            for(int j = 0; j < this.grid[0].Length; j++) 
                if(grid[i][j] == 1) max = Math.Min(max, findPath((i, j, 1, 0)));
            
        return max.ToString();
    }
}