namespace Days;

using DaySetup;
using UtilLibrary;

public class Day11 : Day
{

    private class Monkey {
        Queue<long> items;
        string op;
        int opParam;
        int divider;
        int monkeyTrue;
        int monkeyFalse;
        long inspectCount;

        public Monkey(string definition) {
            string[] lines = definition.Split("\n");
            
            this.items = new Queue<long>();
            foreach(string item in lines[1].Split(": ")[1].Split(", ")) 
                this.items.Enqueue(long.Parse(item));

            this.op = lines[2].Split(" ")[6];
            if(lines[2].Split(" ")[7] == "old") this.op = "^";
            else this.opParam = int.Parse(lines[2].Split(" ")[7]);

            this.divider = int.Parse(lines[3].Split(" ")[5]);
            this.monkeyTrue = int.Parse(lines[4].Split(" ")[9]);
            this.monkeyFalse = int.Parse(lines[5].Split(" ")[9]);
            this.inspectCount = 0;
        }

        public void addItem(long item) {
            this.items.Enqueue(item);
        }

        public long getDivider(){
            return this.divider;
        }

        public long getInspectCount(){
            return this.inspectCount;
        }

        public void makeThrow(Monkey[] monkeys, long lcm, bool regulateStress) {
            while(this.items.Count > 0) {
                long item = this.items.Dequeue();
                this.inspectCount++;

                switch(this.op) {
                    case "+":
                        item += this.opParam;
                    break;
                    case "*":
                        item *= this.opParam;
                    break;
                    default:
                        item = item * item;
                    break;
                }

                item %= lcm;
                if(regulateStress) item = item / 3;
                
                int nextMonkey = item % this.divider == 0 ? monkeyTrue : monkeyFalse;
                monkeys[nextMonkey].addItem(item);
            }
        }
    }


    private string makeSteps(string input, int steps, bool stress) {
        Monkey[] monkeys = input.Split("\n\n").Select(definition => new Monkey(definition)).ToArray();
        long lcm = monkeys.Select(m => m.getDivider()).Aggregate(1, (long lcm, long next) => AoCUtils.Lcm(lcm, next));
        
        foreach(int _ in Enumerable.Repeat(0, steps)) 
            foreach(Monkey m in monkeys) 
                m.makeThrow(monkeys, lcm, stress);
                
        long[] result = monkeys.Select(m => m.getInspectCount()).OrderBy(i => i).Reverse().Take(2).ToArray();
        return (result[0] * result[1]).ToString();
    }

    public override string part1(string input) {
        return makeSteps(input, 20, true);
    }
    
    public override string part2(string input) {
        return makeSteps(input, 10000, false);
    }
}