namespace Days;

using DaySetup;

public class Day17 : Day
{

    List<HashSet<(int x, int y)>> shapes;

    public Day17() {
        shapes = new List<HashSet<(int x, int y)>>();

        shapes.Add(new HashSet<(int x, int y)> {(0, 0), (1, 0), (2, 0), (3, 0)});
        shapes.Add(new HashSet<(int x, int y)> {(0, 1), (1, 0), (1, 1), (1, 2), (2, 1)});
        shapes.Add(new HashSet<(int x, int y)> {(0, 0), (1, 0), (2, 0), (2, 1), (2, 2)});
        shapes.Add(new HashSet<(int x, int y)> {(0, 0), (0, 1), (0, 2), (0, 3)});
        shapes.Add(new HashSet<(int x, int y)> {(0, 0), (1, 0), (0, 1), (1, 1)});
    }

    private bool checkOverlap(HashSet<(int x, int y)> a, HashSet<(int x, int y)> b) {
        return a.Intersect(b).Count() > 0 || a.Where(l => l.x < -2 || l.x > 4).Count() > 0;
    }

    private HashSet<(int x, int y)> translate(HashSet<(int x, int y)> shape, int deltaX, int deltaY) {
        HashSet<(int x, int y)> translated = new HashSet<(int x, int y)>();
        foreach ((int x, int y) coord in shape)
            translated.Add((coord.x + deltaX, coord.y + deltaY));
        return translated;
    }

    private string playTetris(string input, long blocks) {
        input = input.Replace("\n", "");
        int fallen = 0;
        int height = 0;
        HashSet<(int x, int y)> occupied = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> currentShape;
        Dictionary<(int n, int x, int d), (int h, int f)> memState = new Dictionary<(int n, int x, int d), (int h, int f)>();
        int currentAction = 0;
        int dirsCount = input.Length;

        for (int i = -3; i <= 5; i++)
            occupied.Add((i, 0));

        while(true) {
            int shapeIndex = fallen % 5;
            currentShape = this.shapes[shapeIndex].Select(coord => (coord.x, coord.y + height + 4)).ToHashSet();

            while(true) {
                char dir = input[currentAction % dirsCount];
                currentAction++;
                HashSet<(int x, int y)> nextLR;
                HashSet<(int x, int y)> nextD;

                if(dir == '<') nextLR = translate(currentShape, -1, 0);
                else nextLR = translate(currentShape, 1, 0);

                if(checkOverlap(nextLR, occupied)) nextLR = currentShape;

                nextD = translate(nextLR, 0, -1);

                if(checkOverlap(nextD, occupied)) {
                    foreach((int x, int y) coord in nextLR) occupied.Add(coord);
                    height = Math.Max(height, nextLR.Select(c => c.y).Max());
                    fallen++;

                    (int n, int x, int d) mem = (
                        shapeIndex,
                        nextLR.Select(c => c.x).Min(),
                        currentAction % dirsCount
                    );

                    if(memState.Keys.Contains(mem)) {
                        int heightDiff = height - memState[mem].h;
                        int fallenDiff = fallen - memState[mem].f;

                        if((blocks - fallen) % fallenDiff == 0) 
                            return (height + (blocks - fallen) / fallenDiff * heightDiff).ToString();
                        
                    }
                    memState[mem] = (height, fallen);
                    break;
                } else {
                    currentShape = nextD;
                }
            }
        }
    }

    public override string part1(string input) {
        return playTetris(input, 2022);
    }

    public override string part2(string input) {
        return playTetris(input, 1000000000000);
    }
}