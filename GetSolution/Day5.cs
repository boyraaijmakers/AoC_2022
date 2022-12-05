namespace Days;

using DaySetup;

public class Day5 : Day
{
    private Stack<char>[] parseStacks(string encoding) {

        string[] lines = encoding.Split("\n");
        Stack<char>[] towers = new Stack<char>[
            (lines[lines.Length - 1].Length + 1)/4
        ];

        for (int i = 0; i < towers.Length; i++)
            towers[i] = new Stack<char>();
        
        for(int i = lines.Length - 2; i >= 0; i--) {
            for(int j = 0; j < towers.Length; j++) {
                char c = lines[i][(j*4) + 1];
                if(c != ' ') 
                    towers[j].Push(c);
            }
        }

        return towers;
    }

    private string makeMoves(string input, bool first) {
        string[] split = input.Split("\n\n");
        Stack<char>[] towers = parseStacks(split[0]);

        foreach(string move in split[1].Split("\n")) {
            string[] moveSplit = move.Split(" ");
            
            int num = 0;
            int moveNum = int.Parse(moveSplit[1]);

            if(first) {
                while (num < moveNum) {
                    towers[int.Parse(moveSplit[5]) - 1].Push( 
                        towers[int.Parse(moveSplit[3]) - 1].Pop()
                    );
                    num++;
                }
            } else {
                List<char> dummy = new List<char>();

                for(int i = 0; i < moveNum; i++) 
                    dummy.Add(towers[int.Parse(moveSplit[3]) - 1].Pop());

                for(int j = moveNum - 1; j >= 0; j--) 
                    towers[int.Parse(moveSplit[5]) - 1].Push(dummy[j]);
            }
        }

        string result = "";

        foreach(Stack<char> tower in towers) 
            result += tower.Peek();
        
        return result;

    }
    public override string part1(string input) {
        return makeMoves(input, true);
    }

    public override string part2(string input) {
        return makeMoves(input, false);
    }
}
