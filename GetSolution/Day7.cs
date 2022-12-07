namespace Days;

using DaySetup;

public class Day7 : Day
{
    string[] inputLines;
    List<long> folderValues;

    public Day7() {
        inputLines = new string[0];
        folderValues = new List<long>();
    }

    private (long, int) getFolderValue(string folderName, int pointer) {
        long folderTotal = 0;
        for(int i = pointer; i < this.inputLines.Length; i++) {
            if(this.inputLines[i] == "$ cd ..") {
                folderValues.Add(folderTotal);
                return (folderTotal, i);
            }
            if(this.inputLines[i] == "$ ls") continue;

            string[] lineParts = this.inputLines[i].Split(" ");

            switch(lineParts[0]) {
                case "$":
                    (long subTotal, i) = getFolderValue(lineParts[2], i+1);
                    folderTotal += subTotal;
                break;
                case "dir":
                break;

                default:
                    folderTotal += long.Parse(lineParts[0]);
                break;
            }
        }
        return (folderTotal, this.inputLines.Length);
    }

    private long getRootTotal(string input) {
        this.inputLines = input.Split("\n");
        (long rootTotal, _) = getFolderValue("/", 1);
        return rootTotal;
    }

    public override string part1(string input) {
        getRootTotal(input);
        return folderValues.Where(l => l < 100000).Sum().ToString();
    }

    public override string part2(string input) {
        long required = getRootTotal(input) - 40000000;
        return folderValues.Where(l => l > required).Min().ToString();
    }
}
