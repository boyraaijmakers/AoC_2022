namespace Days;

using DaySetup;

public class Day23 : Day
{  
    private HashSet<(int x, int y)> parseInput(string input) {
        HashSet<(int x, int y)> elves = new HashSet<(int x, int y)>();
        string[] lines = input.Split("\n");
        for(int i = 0; i < lines.Length; i++) {
            char[] parts = lines[i].ToCharArray();
            for(int j = 0; j < parts.Length; j++) 
                if(parts[j] == '#') elves.Add((i, j));
        }
        return elves;
    }
    
    private Dictionary<(int x, int y), List<(int x, int y)>> getProposals(HashSet<(int x, int y)> elves, int initMove) {
        Dictionary<(int x, int y), List<(int x, int y)>> proposals = new Dictionary<(int x, int y), List<(int x, int y)>>();
        Dictionary<int, List<(int x, int y)>> checks = new Dictionary<int, List<(int x, int y)>>() {
            {0, new List<(int x, int y)>() {(-1, -1), (-1, 0), (-1, 1)}},
            {1, new List<(int x, int y)>() {(1, -1), (1, 0), (1, 1)}},
            {2, new List<(int x, int y)>() {(-1, -1), (0, -1), (1, -1)}},
            {3, new List<(int x, int y)>() {(-1, 1), (0, 1), (1, 1)}}
        };

        foreach((int x, int y) elf in elves) {
            bool foundSpot = false;
            bool shouldMove = false;
            int move = initMove;

            for(int i = -1; i <= 1; i++) {
                for(int j = -1; j <= 1; j++) {
                    if(i==0 && j==0) continue;
                    if(elves.Contains((elf.x+i, elf.y+j))) shouldMove = true;
                }
            }

            if(shouldMove) {
                for(int i = 0; i < 4; i++) {
                    bool free = true;
                    
                    foreach((int x, int y) delta in checks[move]) 
                        if(elves.Contains((elf.x + delta.x, elf.y + delta.y))) free = false;
                    
                    if(free) {
                        foundSpot = true;
                        (int x, int y) newElf = (elf.x + checks[move][1].x, elf.y + checks[move][1].y);
                        if (proposals.Keys.Contains(newElf)) proposals[newElf].Add(elf);
                        else proposals[newElf] = new List<(int x, int y)>() {elf};
                        break;
                    }
                    
                    move = (move + 1) % 4;
                }
            }
            
            if(!foundSpot) {
                if (proposals.Keys.Contains(elf)) proposals[elf].Add(elf);
                else proposals[elf] = new List<(int x, int y)>() {elf};
            }
        }

        return proposals;
    }
    
    private string doMoves(string input, bool part1) {
        HashSet<(int x, int y)> elves = parseInput(input);

        int steps = 1;
        int move = 0;

        while(part1 && steps <= 10 || !part1) {
            Dictionary<(int x, int y), List<(int x, int y)>> proposals = getProposals(elves, move);
            HashSet<(int x, int y)> newElves = new HashSet<(int x, int y)>();

            if(!part1 && proposals.Where(item => item.Value.Count > 1 || item.Value.Count == 1 && item.Key != item.Value[0]).Count() == 0)
                    return steps.ToString();

            foreach((int x, int y) newPos in proposals.Keys) {
                if (proposals[newPos].Count == 1) newElves.Add(newPos);
                else foreach((int x, int y) oldPos in proposals[newPos]) newElves.Add(oldPos);
            }
            
            steps++;
            elves = newElves;
            move = (move + 1) % 4;           
        }
       
        int maxX = elves.Select(elf => elf.x).Max();
        int minX = elves.Select(elf => elf.x).Min();
        int maxY = elves.Select(elf => elf.y).Max();
        int minY = elves.Select(elf => elf.y).Min();

        return ((maxX - minX + 1) * (maxY - minY + 1) - elves.Count).ToString();
    }
    public override string part1(string input) {
        return doMoves(input, true);
    }
   
    public override string part2(string input) {
        return doMoves(input, false);
    }
}