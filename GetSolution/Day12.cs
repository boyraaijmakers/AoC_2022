namespace Days;

using DaySetup;
using UtilLibrary;

public class Day12 : Day
{
    
    (int x, int y) start;
    (int x, int y) end;

    
    private int findPath((int x, int y, int previousLevel, int steps) start, int[][] grid) {
        Queue<(int x, int y, int previousLevel, int steps)> items;
        items = new Queue<(int x, int y, int currentLevel, int steps)>();
        items.Enqueue(start);

        Dictionary<(int x, int y), int> paths = new Dictionary<(int x, int y), int>();

        while(items.Count > 0) {
            (int x, int y, int previousLevel, int steps) next = items.Dequeue();
            if(paths.ContainsKey((next.x, next.y))) if(paths[(next.x, next.y)] <= next.steps) continue;

            int currentLevel = grid[next.x][next.y];
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

    private int[][] parseGrid(string input) {
        int[][] grid = input.Split("\n").Select(
            line => line.ToCharArray().Select(c => (int)c).ToArray()
        ).ToArray();

        for(int i = 0; i < grid.Length; i++) {
            for(int j = 0; j < grid[0].Length; j++) {
                if (grid[i][j] == 83) {
                    this.start = (i, j);
                    grid[i][j] = 1;
                }
                if (grid[i][j] == 69) {
                    this.end = (i, j);
                    grid[i][j] = 26;
                }

                grid[i][j] %= 32;
            }
        }

        return grid;
    }

    public override string part1(string input) {
        return findPath((this.start.x, this.start.y, 1, 0), parseGrid(input)).ToString();
    }
    
    public override string part2(string input) {
        int[][] grid = parseGrid(input);

        int max = grid.Length * grid[0].Length;
        for(int i = 0; i < grid.Length; i++) 
            for(int j = 0; j < grid[0].Length; j++) 
                if(grid[i][j] == 1) max = Math.Min(max, findPath((i, j, 1, 0), grid));
            
        return max.ToString();
    }
}