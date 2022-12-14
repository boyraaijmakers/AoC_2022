namespace Days;

using DaySetup;

public class Day14 : Day
{
    private HashSet<(int x, int y)> getWalls(string input) {
        string[] lines = input.Split("\n");
        HashSet<(int x, int y)> walls = new HashSet<(int x, int y)>();

        foreach(string line in lines) {
            (int x, int y)[] lineParts = line.Split(" -> ").Select(
                s => (int.Parse(s.Split(",")[0]), int.Parse(s.Split(",")[1]))
            ).ToArray();

            (int x, int y) current = lineParts[0];
            walls.Add(current);

            for(int i = 1; i < lineParts.Length; i++) {
                (int x, int y) next = lineParts[i];

                while (current != next) {
                    current = (
                        current.x + Math.Sign(next.x - current.x),
                        current.y + Math.Sign(next.y - current.y)
                    );
                    walls.Add(current);
                }
                current = next;
            }
        }
        return walls;
    }

    private string getMaxSand(string input, bool part1) {
        HashSet<(int x, int y)> occupied = getWalls(input);

        int maxY = occupied.Select(item => item.y).Max();
        int wallSize = occupied.Count;

        bool done = false;
        while(!done) {
            (int x, int y) sand = (500, 0);

            while (true) {
                if(!occupied.Contains((sand.x, sand.y + 1))) sand = (sand.x, sand.y+1);
                else if(!occupied.Contains((sand.x-1, sand.y+1))) sand = (sand.x-1, sand.y+1);
                else if(!occupied.Contains((sand.x+1, sand.y+1))) sand = (sand.x+1, sand.y+1);
                else {
                    occupied.Add(sand);
                    if(!part1 && sand == (500, 0)) done = true;
                    break;
                }

                if(part1 && sand.y > maxY) {
                    done = true;
                    break;
                } else if (!part1 && sand.y == maxY+1) {
                    occupied.Add(sand);
                    break;
                }
            }
        }
        return (occupied.Count - wallSize).ToString();
    }

    public override string part1(string input) {
        return getMaxSand(input, true);
    }

    public override string part2(string input) {
        return getMaxSand(input, false);
    }
}