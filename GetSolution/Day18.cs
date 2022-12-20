namespace Days;

using DaySetup;

public class Day18 : Day
{

    public override string part1(string input) {
        HashSet<(int x, int y, int z)> cubes = 
            input.Split("\n").Select(line => (
                int.Parse(line.Split(",")[0]),
                int.Parse(line.Split(",")[1]),
                int.Parse(line.Split(",")[2])
            )).ToHashSet();

        int sides = cubes.Count * 6;

        foreach((int x, int y, int z) cube in cubes) {
            if(cubes.Contains((cube.x + 1, cube.y, cube.z))) sides--;
            if(cubes.Contains((cube.x - 1, cube.y, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y + 1, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y - 1, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y, cube.z + 1))) sides--;
            if(cubes.Contains((cube.x, cube.y, cube.z - 1))) sides--;
        }
        return sides.ToString();
    }

    private bool checkExterior(
            HashSet<(int x, int y, int z)> exteriorBlocks, 
            HashSet<(int x, int y, int z)> cubes,
            (int x, int y, int z) b) {
        
        int delta = 1;
        while(true) {
            if(exteriorBlocks.Contains((b.x-delta, b.y, b.z))) return true;
            if(cubes.Contains((b.x-delta, b.y, b.z))) break;
            if (b.x - delta < 0) break;
            delta++;
        }

        delta = 1;
        while(true) {
            if(exteriorBlocks.Contains((b.x+delta, b.y, b.z))) return true;
            if(cubes.Contains((b.x+delta, b.y, b.z))) break;
            if (b.x + delta > 20) break;
            delta++;
        }

        delta=1;
        while(true) {
            if(exteriorBlocks.Contains((b.x, b.y-delta, b.z))) return true;
            if(cubes.Contains((b.x, b.y-delta, b.z))) break;
            if (b.y - delta < 0) break;
            delta++;
        }

        delta = 1;
        while(true) {
            if(exteriorBlocks.Contains((b.x, b.y+delta, b.z))) return true;
            if(cubes.Contains((b.x, b.y+delta, b.z))) break;
            if (b.y + delta > 20) break;
            delta++;
        }

        delta = 1;
        while(true) {
            if(exteriorBlocks.Contains((b.x, b.y, b.z-delta))) return true;
            if(cubes.Contains((b.x, b.y, b.z-delta))) break;
            if (b.z - delta < 0) break;
            delta++;
        }

        delta=1;
        while(true) {
            if(exteriorBlocks.Contains((b.x, b.y, b.z + delta))) return true;
            if(cubes.Contains((b.x, b.y, b.z + delta))) break;
            if (b.z + delta > 20) break;
            delta++;
        }

        return false;
    }

    public override string part2(string input) {
        HashSet<(int x, int y, int z)> cubes = 
            input.Split("\n").Select(line => (
                int.Parse(line.Split(",")[0]),
                int.Parse(line.Split(",")[1]),
                int.Parse(line.Split(",")[2])
            )).ToHashSet();
        
        int sides = cubes.Count * 6;

        HashSet<(int x, int y, int z)> exteriorBlocks = new HashSet<(int x, int y, int z)>();
        HashSet<(int x, int y, int z)> interiorBlocks = new HashSet<(int x, int y, int z)>();

        for(int j = 0; j <=20; j++) 
            for(int k = 0; k <= 20; k++) 
                exteriorBlocks.Add((0, j, k));
            
         for(int j = 0; j <=20; j++) 
            for(int k = 0; k <= 20; k++) 
                exteriorBlocks.Add((j, 0, k));

         for(int j = 0; j <=20; j++) 
            for(int k = 0; k <= 20; k++) 
                exteriorBlocks.Add((k, j, 0));


        for(int i = 1; i < 20; i++) {
            for(int j = 1; j < 20; j++) {
                for(int k = 1; k < 20; k++) {
                    if(!cubes.Contains((i, j, k))) {
                        if(checkExterior(exteriorBlocks, cubes, (i, j, k))) exteriorBlocks.Add((i, j, k));
                        else interiorBlocks.Add((i, j, k));
                    }
                }
            }
        }

        while(true) {
            HashSet<(int x, int y, int z)> newInteriors = new HashSet<(int x, int y, int z)>(interiorBlocks);
            foreach((int x, int y, int z) cube in interiorBlocks) {
                if(checkExterior(exteriorBlocks, cubes, cube)) {
                    newInteriors.Remove(cube);
                    exteriorBlocks.Add(cube);
                }
            }
            if(newInteriors.Count == interiorBlocks.Count) break;
            interiorBlocks = newInteriors;
        }
        

        foreach((int x, int y, int z) cube in cubes) {
            if(cubes.Contains((cube.x + 1, cube.y, cube.z))) sides--;
            if(cubes.Contains((cube.x - 1, cube.y, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y + 1, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y - 1, cube.z))) sides--;
            if(cubes.Contains((cube.x, cube.y, cube.z + 1))) sides--;
            if(cubes.Contains((cube.x, cube.y, cube.z - 1))) sides--;

            if(interiorBlocks.Contains((cube.x + 1, cube.y, cube.z))) sides--;
            if(interiorBlocks.Contains((cube.x - 1, cube.y, cube.z))) sides--;
            if(interiorBlocks.Contains((cube.x, cube.y + 1, cube.z))) sides--;
            if(interiorBlocks.Contains((cube.x, cube.y - 1, cube.z))) sides--;
            if(interiorBlocks.Contains((cube.x, cube.y, cube.z + 1))) sides--;
            if(interiorBlocks.Contains((cube.x, cube.y, cube.z - 1))) sides--;
        }

        return sides.ToString(); 
    }
}