namespace Days;

using System.Numerics;
using DaySetup;

public class Day21 : Day
{  
   
    private Dictionary<string, (string a, string b, string op)> recurse;
    private Dictionary<string, BigInteger> shouts;

    public Day21() {
        recurse = new Dictionary<string, (string a, string b, string op)>();
        shouts = new Dictionary<string, BigInteger>();
    }

    private void parseInput(string input) {
        foreach(string line in input.Split("\n")) {
            string[] lineParts = line.Split(": ");

            if(lineParts[1].Split(" ").Length == 1) 
                this.shouts[lineParts[0]] = int.Parse(lineParts[1]); 
            else {
                string[] recurseParts = lineParts[1].Split(" ");
                this.recurse[lineParts[0]] = (recurseParts[0], recurseParts[2], recurseParts[1]);
            }
        }
    }

    private BigInteger getValue(string item) {
        if(this.shouts.Keys.Contains(item)) return this.shouts[item];

        (string a, string b, string op) = this.recurse[item];

        BigInteger resA = getValue(a);
        BigInteger resB = getValue(b);

        switch(op) {
            case "+":
                return resA + resB;
            case "-":
                return resA - resB;
            case "*":
                return resA * resB;
            case "/":
                return resA / resB;
            default:
                Console.WriteLine("Poor format!");
                return -1;
        }
    }

    public override string part1(string input) {
        parseInput(input);
        return getValue("root").ToString();
    }
   
    public override string part2(string input) {
        parseInput(input);

        long yourShout = long.MaxValue / 2;
        (long lower, long upper) bounds = (0, long.MaxValue);

        (string a, string b, string op) check = this.recurse["root"];
        BigInteger resB = getValue(check.b);

        while(true) {
            shouts["humn"] = yourShout;
            
            BigInteger resA = getValue(check.a);
            if(resA == resB) return yourShout.ToString();

            else if(resA > resB) bounds.lower = yourShout;
            else bounds.upper = yourShout;
            
            yourShout = (bounds.upper + bounds.lower) / 2;
        }
    }
}