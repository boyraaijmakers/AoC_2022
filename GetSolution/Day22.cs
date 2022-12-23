namespace Days;

using DaySetup;

public class Day22 : Day
{  
    
    private int doWalk(HashSet<int> tiles, HashSet<int> walls, int point, int dir, int steps) {
        while(steps > 0) {
            int prevPoint = point;
            point += dir;
            steps--;

            if(!tiles.Contains(point)) {
                int x = point / 1000;
                int y = point % 1000;
                y = (y < 0) ? y + 1000 : y;

                if(dir > 0) {
                    if(dir == 1) point = tiles.Where(coord => coord >= x * 1000 && coord < (x+1) * 1000).Min();
                    else point = tiles.Where(coord => coord % 1000 == y).Min();
                } else {
                    if(dir == -1) point = tiles.Where(coord => coord >= x * 1000 && coord < (x+1) * 1000).Max();
                    else point = tiles.Where(coord => coord % 1000 == y).Max();
                }
            }

            if(walls.Contains(point)) return prevPoint;
        }

        return point;
    }

    private int newCoord(int prevCoord, int prevEdge, int newEdge, bool reverse) {
        int x = prevCoord / 1000;
        int y = prevCoord % 1000;
        int newY = x;
        int newX = y;
        
        switch(newEdge) {
            case 1:
                newY = 1;
                switch(prevEdge) {
                    case -1:
                        newX = 51 - x;
                    break;
                    case -1000:
                        newX = reverse ? 51 - y : y;
                    break;
                    case 1:
                        newX = x;
                    break;
                }
            break;
            case 1000:
                newX = 1;
                switch(prevEdge) {
                    case -1:
                        newY = reverse ? 51 - x : x;
                    break;
                    case 1000:
                        newY = y;
                    break;
                }
            break;
            case -1:
                newY = 50;
                switch(prevEdge) {
                    case -1:
                        newX = x;
                    break;
                    case 1:
                        newX = 51 - x;
                    break;
                    case 1000:
                        newX = y;
                    break;
                }
            break;
            case -1000:
                newX = 50;
                switch(prevEdge) {
                    case -1000:
                        newY = y;
                    break;
                    case 1:
                        newY = x;
                    break;
                }
            break;
        }

        return newX * 1000 + newY;
    }

    private (int side, int coord, int newDir) doCubeWalk(
                Dictionary<int, HashSet<int>> tiles, Dictionary<int, HashSet<int>> walls, 
                Dictionary<int, (int side, int border, int newDir, bool inverted)[]> sideMapping,
                (int side, int coord, int dir) point, int steps) {
        
        while(steps > 0) {
            (int side, int coord, int dir) prevPoint = point;
            point.coord += point.dir;
            steps--;

            if(!tiles[point.side].Contains(point.coord)) {
                int x = point.coord / 1000;
                int y = point.coord % 1000;
                y = (y < 0) ? y + 1000 : y;
                (int side, int border, int newDir, bool inverted) newSide;

                switch(point.dir) {
                    case -1:
                        newSide = sideMapping[point.side][0];
                        point.dir = newSide.newDir;
                        point.side = newSide.side; 
                        point.coord = newCoord(prevPoint.coord, prevPoint.dir, point.dir, newSide.inverted);                  
                    break;
                    case -1000:
                        newSide = sideMapping[point.side][1];
                        point.dir = newSide.newDir;
                        point.side = newSide.side;
                        point.coord = newCoord(prevPoint.coord, prevPoint.dir, point.dir, newSide.inverted);
                    break;
                    case 1:
                        newSide = sideMapping[point.side][2];
                        point.dir = newSide.newDir;
                        point.side = newSide.side;
                        point.coord = newCoord(prevPoint.coord, prevPoint.dir, point.dir, newSide.inverted);
                    break;
                    case 1000:
                        newSide = sideMapping[point.side][3];
                        point.dir = newSide.newDir;
                        point.side = newSide.side;
                        point.coord = newCoord(prevPoint.coord, prevPoint.dir, point.dir, newSide.inverted);
                    break;
                    default:
                        Console.WriteLine("Unknown dir!");
                    break;
                }
            }

            if(walls[point.side].Contains(point.coord)) return prevPoint;
        }

        return point;
    }
    
    public override string part1(string input) {
        string[] inputParts = input.Split("\n\n");
        string grid = inputParts[0];
        string instructionString = inputParts[1];
        
        HashSet<int> tiles = new HashSet<int>();
        HashSet<int> walls = new HashSet<int>();

        string[] gridRows = grid.Split("\n");

        for(int i = 0; i < gridRows.Length; i++) {
            char[] gridLine = gridRows[i].ToCharArray();

            for(int j = 0; j < gridLine.Length; j++) {
                if (gridLine[j] != ' ') tiles.Add((i+1) * 1000 + j+1);
                if (gridLine[j] == '#') walls.Add((i+1) * 1000 + j+1);
            }
        }

        int point = tiles.Min();

        string parsed = "";
        int dir = 1;

        char[] instructions = instructionString.ToCharArray();

        foreach(char c in instructions) {
            switch(c) {
                case 'L':
                    point = doWalk(tiles, walls, point, dir, int.Parse(parsed));
                    dir = -(dir % 1000) * 1000 + dir / 1000;
                    parsed = "";
                break;
                case 'R':
                    point = doWalk(tiles, walls, point, dir, int.Parse(parsed));
                    dir = (dir % 1000) * 1000 - dir / 1000;
                    parsed = "";
                break;
                default:
                    parsed += c;
                break;
            }
        }

        point = doWalk(tiles, walls, point, dir, int.Parse(parsed));

        int dirResult = dir > 0 ? (dir + 1) % 2 : 3 + (dir % 2);
        return ((point / 1000) * 1000 + point % 1000 * 4 + dirResult).ToString();
    }
   
    public override string part2(string input) {
        string[] inputParts = input.Split("\n\n");
        string grid = inputParts[0];
        string instructionString = inputParts[1];
        
        Dictionary<int, HashSet<int>> tiles = new Dictionary<int, HashSet<int>>();
        Dictionary<int, HashSet<int>> walls = new Dictionary<int, HashSet<int>>();

        for(int i = 1; i <= 6; i++) {
            tiles[i] = new HashSet<int>();
            walls[i] = new HashSet<int>();
        }

        Dictionary<int, int[]> inputMapping = new Dictionary<int, int[]>() {
            { 1, new int[] {0, 50}},
            { 2, new int[] {0, 100}},
            { 3, new int[] {50, 50}},
            { 4, new int[] {100, 50}},
            { 5, new int[] {100, 0}},
            { 6, new int[] {150, 0}},
        };

        Dictionary<int, (int side, int border, int newDir, bool inverted)[]> sideMapping = new Dictionary<int, (int side, int border, int newDir, bool inverted)[]>() {
            {1, new (int side, int border, int newDir, bool inverted)[] {
                (5, 1, 1, true),
                (6, 1, 1, false),
                (2, 1, 1, false), 
                (3, 2, 1000, false)
            }},
            {2, new (int side, int border, int newDir, bool inverted)[] {
                (1, 3, -1, false), 
                (6, 4, -1000, false),
                (4, 3, -1, true),
                (3, 3, -1, false)
            }},
            {3, new (int side, int border, int newDir, bool inverted)[] {
                (5, 2, 1000, false), 
                (1, 4, -1000, false),
                (2, 4, -1000, false),
                (4, 2, 1000, false)
            }},
            {4, new (int side, int border, int newDir, bool inverted)[] {
                (5, 3, -1, false), 
                (3, 4, -1000, false),
                (2, 3, -1, true),
                (6, 3, -1, false)
            }},
            {5, new (int side, int border, int newDir, bool inverted)[] {
                (1, 1, 1, true), 
                (3, 1, 1, false),
                (4, 1, 1, false),
                (6, 2, 1000, false)
            }},
            {6, new (int side, int border, int newDir, bool inverted)[] {
                (1, 2, 1000, false), 
                (5, 4, -1000, false),
                (4, 4, -1000, false),
                (2, 2, 1000, false)
            }}
        };

        string[] gridRows = grid.Split("\n");

        foreach(int side in inputMapping.Keys) {
            for(int i = 0; i < 50; i++) {
                char[] gridLine = gridRows[i + inputMapping[side][0]].ToCharArray();

                for(int j = 0; j < 50; j++) {
                    if (gridLine[j + inputMapping[side][1]] != ' ') tiles[side].Add((i+1) * 1000 + j+1);
                    if (gridLine[j + inputMapping[side][1]] == '#') walls[side].Add((i+1) * 1000 + j+1);
                }
            }
        }

        (int side, int coord, int dir) point = (1, tiles[1].Min(), 1);

        string parsed = "";
        char[] instructions = instructionString.ToCharArray();

        foreach(char c in instructions) {
            switch(c) {
                case 'L':
                    point = doCubeWalk(tiles, walls, sideMapping, point, int.Parse(parsed));
                    point.dir = -(point.dir % 1000) * 1000 + point.dir / 1000;
                    parsed = "";
                break;
                case 'R':
                    point = doCubeWalk(tiles, walls, sideMapping, point, int.Parse(parsed));
                    point.dir = (point.dir % 1000) * 1000 - point.dir / 1000;
                    parsed = "";
                break;
                default:
                    parsed += c;
                break;
            }
        }

        point = doCubeWalk(tiles, walls, sideMapping, point, int.Parse(parsed));
        
        int finalX = point.coord / 1000;
        int finalY = point.coord % 1000;
        
        finalX += inputMapping[point.side][0];
        finalY += inputMapping[point.side][1];

        int dirResult = point.dir > 0 ? (point.dir + 1) % 2 : 3 + (point.dir % 2);

        return (finalX * 1000 + finalY * 4 + dirResult).ToString();
    }
}