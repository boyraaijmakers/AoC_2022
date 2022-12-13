namespace Days;

using DaySetup;

public class Day13 : Day
{
    private string[] tokenize(string listStr) {
        List<string> tokens = new List<string>();

        int lefts = 0;
        int rights = 0;
        int prevI = 1;

        for(int i = 1; i < listStr.Length; i++){
            switch(listStr[i]) {
                case '[':
                    lefts++;
                break;
                case ']':
                    rights++;
                break;
                case ',':
                    if(lefts == rights) {
                        tokens.Add(listStr.Substring(prevI, i - prevI));
                        prevI = i + 1;
                    }
                break;
                default: 
                break;
            }
        }

        if (prevI != listStr.Length - prevI)
            tokens.Add(listStr.Substring(prevI, listStr.Length - 1 - prevI));

        return tokens.ToArray();
    }
    
    private int checkPair(string left, string right) {
        string[] tokensLeft = tokenize(left);
        string[] tokensRight = tokenize(right);

        if(tokensLeft.Count() == 0) return tokensRight.Count() == 0 ? 0 : 1;
        
        for(int i = 0; i < tokensLeft.Count(); i++) {
            if(i == tokensRight.Count()) return -1;

            string leftToken = tokensLeft[i];
            string rightToken = tokensRight[i];

            if(leftToken[0] == '[' && rightToken[0] != '[') 
                rightToken = "[" + rightToken + "]";
                
            if(rightToken[0] == '[' && leftToken[0] != '[')
                leftToken = "[" + leftToken + "]";

            if(leftToken[0] != '[' && rightToken[0] != '[' ){
                int sign = int.Parse(leftToken) - int.Parse(rightToken);
                if (sign < 0) return 1;
                if (sign > 0) return -1;
            } 

            if(leftToken[0] == '[') {
                int res = checkPair(leftToken, rightToken);
                if (res != 0) return res;
            }           
        }
        
        return tokensLeft.Count() == tokensRight.Count() ? 0 : 1;
    }

    public override string part1(string input) {
        int sum = 0;
        string[] pairs = input.Split("\n\n");
        
        for (int i = 0; i < pairs.Count(); i++) 
            if(checkPair(pairs[i].Split("\n")[0], pairs[i].Split("\n")[1]) == 1) 
                sum += i + 1;
            
        return sum.ToString();
    }

    public override string part2(string input) {
        input += "\n[[2]]\n[[6]]";

        string[] ordered = input.Split("\n").Where(line => line != "").ToArray();
        Array.Sort(ordered, checkPair);
        Array.Reverse(ordered);

        int prod = 1;

        for(int i = 0; i < ordered.Length; i++) 
            if (ordered[i] == "[[2]]" || ordered[i] == "[[6]]") 
                prod *= i+1;
        
        return prod.ToString();
    }
}