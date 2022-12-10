namespace Days;

using DaySetup;

public class Day8 : Day
{
    public override string part1(string input) {
        string[] inputLines = input.Split("\n");
        int[][] grid = inputLines.Select(line => line.ToCharArray().Select(item => item - '0').ToArray()).ToArray();
        HashSet<string> visisbleTrees = new HashSet<string>();
        
        int height = grid.Length;
        int width = grid[0].Length;
        int treesVisible = (height + width - 2) * 2;
        
        // left to right
        for (int i = 1; i < height - 1; i++) {
            int highestTree = grid[i][0];
            for (int j = 1; j < width - 1; j++) {
                if (grid[i][j] > highestTree) {
                    highestTree = grid[i][j];
                    visisbleTrees.Add(i + "," + j);
                    if (highestTree == 9) break;
                }
            }
        }

        // right to left
        for (int i = 1; i < height - 1; i++) {
            int highestTree = grid[i][width - 1];
            for (int j = width - 2; j > 0; j--) {
                if (grid[i][j] > highestTree) {
                    highestTree = grid[i][j];
                    visisbleTrees.Add(i + "," + j);
                    if (highestTree == 9) break;
                }
            }
        }

        // top to bottom
        for (int i = 1; i < width - 1; i++) {
            int highestTree = grid[0][i];
            for (int j = 1; j < height - 1; j++) {
                if (grid[j][i] > highestTree) {
                    highestTree = grid[j][i];
                    visisbleTrees.Add(j + "," + i);
                    if (highestTree == 9) break;
                }
            }
        }

        // bottom to top
        for (int i = 1; i < width - 1; i++) {
            int highestTree = grid[height - 1][i];
            for (int j = height - 2; j > 0; j--) {
                if (grid[j][i] > highestTree) {
                    highestTree = grid[j][i];
                    visisbleTrees.Add(j + "," + i);
                    if (highestTree == 9) break;
                }
            }
        }

        return (treesVisible + visisbleTrees.Count()).ToString();
    }


    private long getScenicScore(int[][] grid, int row, int column) {
        int height = grid.Length;
        int width = grid[0].Length;
        long score = 1;
        int currentTree = grid[row][column];

        // left
        int highestTree = grid[row][column-1];
        long visible = 1;
        if(highestTree < currentTree) 
            for (int j = column - 2; j >= 0; j--) {
                visible++;
                if (grid[row][j] >= currentTree) break;
            }
        
        
        score *= visible;

        // right
        highestTree = grid[row][column+1];
        visible = 1;
        if(highestTree < currentTree) 
            for (int j = column + 2; j < width; j++) {
                visible++;
                if (grid[row][j] >= currentTree) break;
            }
        score *= visible;

        // up
        highestTree = grid[row-1][column];
        visible = 1;
        if(highestTree < currentTree) 
            for (int i = row - 2; i >= 0; i--) {
                visible++;
                if (grid[i][column] >= currentTree) break;
            }
        score *= visible;

        // down
        highestTree = grid[row+1][column];
        visible = 1;
        if(highestTree < currentTree) 
            for (int i = row + 2; i < height; i++)  {
                visible++;
                if (grid[i][column] >= currentTree) break;
            }
        score *= visible;

        return score;
    }

    public override string part2(string input) {
        string[] inputLines = input.Split("\n");
        int[][] grid = inputLines.Select(line => line.ToCharArray().Select(item => item - '0').ToArray()).ToArray();
        long maxScore = 0;

        for (int row = 1; row < grid.Length - 1; row++)
            for (int column = 1; column < grid[0].Length - 1; column++) {
                long score = getScenicScore(grid, row, column);
                maxScore = maxScore < score ? score: maxScore;
            }

        return maxScore.ToString();
                
    }
}
