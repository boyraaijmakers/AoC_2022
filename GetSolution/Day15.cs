namespace Days;

using System.Numerics;
using DaySetup;

public class Day15 : Day
{

    private (int x, int y, int bX, int bY)[] parseInput(string input) {
        return input.Split("\n").Select(line => (
            int.Parse(line.Split(" ")[2].Replace("x=", "").Replace(",", "")),
            int.Parse(line.Split(" ")[3].Replace("y=", "").Replace(":", "")),
            int.Parse(line.Split(" ")[8].Replace("x=", "").Replace(",", "")),
            int.Parse(line.Split(" ")[9].Replace("y=", ""))
        )).ToArray();
    }

    private int manDist((int x, int y) a, (int x, int y) b) {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }


    private HashSet<(int x, int y)> getImpossibleTiles((int x, int y, int bX, int bY)[] sensors, int split) {
        HashSet<(int x, int y)> tiles = new HashSet<(int x, int y)>();

        foreach ((int x, int y, int bX, int bY) sensor in sensors) {
            int distance = manDist((sensor.x, sensor.y), (sensor.bX, sensor.bY));

            if(
                (sensor.y < split && sensor.y + distance >= split) || 
                (sensor.y >= split && sensor.y - distance <= split)
            ) {
                int xLeft = distance - Math.Abs(sensor.y - split);
                for(int i = -xLeft; i <= xLeft; i++) 
                    tiles.Add((sensor.x + i, split));

            }
        }
        
        foreach ((int x, int y, int bX, int bY) sensor in sensors) {
            tiles.Remove((sensor.x, sensor.y));
            tiles.Remove((sensor.bX, sensor.bY));
        }
        
        return tiles;
    }


    private bool checkPoint((int x, int y, int dist)[] sensors, (int x, int y) point) {
        foreach((int x, int y, int dist) sensor in sensors)
            if (manDist((sensor.x, sensor.y), point) <= sensor.dist)
                return false;
        return true;
    }
    private (int x, int y) checkBorders((int x, int y, int dist)[] sensors) {
        foreach((int x, int y, int dist) sensor in sensors) {
            for(int i = -sensor.dist - 1; i <= sensor.dist + 1; i++) {
                if (sensor.x + i < 0 || sensor.x + i > 4000000) continue;
                int j = sensor.dist + 1 - Math.Abs(i);

                if (sensor.y + j >= 0 && sensor.y + j <= 4000000 &&
                    checkPoint(sensors, (sensor.x + i, sensor.y + j))) 
                        return (sensor.x + i, sensor.y + j);
                    
                if (sensor.y - j >= 0 && sensor.y - j <= 4000000 &&
                    checkPoint(sensors, (sensor.x + i, sensor.y - j)))
                        return (sensor.x + i, sensor.y - j);
            }
        }

        return (-1, -1);
    }

    public override string part1(string input) {
        return getImpossibleTiles(parseInput(input), 2000000)
            .Where(coord => coord.y == 2000000)
            .Count()
            .ToString();
    }

    public override string part2(string input) {
        (int x, int y) beacon = checkBorders(
            parseInput(input)
                .Select(sensor => (sensor.x, sensor.y, manDist((sensor.x, sensor.y), (sensor.bX, sensor.bY))))
                .ToArray()
        );
        
        BigInteger solution = beacon.x;
        return (solution * 4000000 + beacon.y).ToString();
    }
}