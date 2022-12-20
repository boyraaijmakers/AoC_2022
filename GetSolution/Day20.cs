namespace Days;

using DaySetup;

public class Day20 : Day
{  
    private class LinkedPoint {
        public long value;
        public LinkedPoint left;
        public LinkedPoint right;

        public LinkedPoint(long value, LinkedPoint left, LinkedPoint right) {
            this.value = value;
            this.left = left;
            this.right = right;
        } 

        public void moveRight() {
            LinkedPoint newRight = this.right.right;

            this.right.right.left = this;
            this.right.left = this.left;
            this.right.right = this;
            

            this.left.right = this.right;
            this.left = this.right;
            this.right = newRight;
        }

        public void moveLeft() {
            LinkedPoint newLeft = this.left.left;
            this.left.left.right = this;
            this.left.right = this.right;
            this.left.left = this;

            this.right.left = this.left;
            this.right = this.left;
            this.left = newLeft;
        }

        public override string ToString() {
            return "(" + this.value + ", L: " + this.left.value + ", R: " + this.right.value + ")";
        }
    }

    private string getResult(string input, long factor, long deduplicateFact, int repeats) {
        long[] lines = input.Split("\n").Select(line => long.Parse(line) * factor).ToArray();
        Dictionary<long, int> dupl = new Dictionary<long, int>();

        for(int i = 0; i < lines.Length; i++) {
            long val = lines[i];
            if(! dupl.Keys.Contains(val)) dupl[val] = 0;
            lines[i] = val + Math.Sign(val) * deduplicateFact * dupl[val];
            dupl[val]++;
        }

        Dictionary<long, LinkedPoint> points = new Dictionary<long, LinkedPoint>();

        // Empty init
        foreach(long i in lines) 
            points[i] = new LinkedPoint(i, null, null);

        // Fill neighbours
        for(int i = 0; i < lines.Length; i++) {
            points[lines[i]].left = points[lines[(i == 0 ? lines.Length  : i) - 1]];
            points[lines[i]].right = points[lines[(i+1) % lines.Length]];
        }

        // And off we go
        int repeat = repeats;
        while(repeat > 0) {
            repeat--;
            foreach(long i in lines) {
                LinkedPoint p = points[i];
                long moves = (Math.Abs(i) % deduplicateFact) % 4999;
                if (i > 0) for(long l1 = 0; l1 < moves ; l1++) p.moveRight();
                else for(long l2 = 0; l2 < moves ; l2++) p.moveLeft();
            }
        }


        LinkedPoint result = points[0];
        long sum = 0;

        for(int i = 0; i < 3000; i++){
            result = result.right;
            if((i + 1) % 1000 == 0) sum += result.value % deduplicateFact;
        }
            
        return sum.ToString();
    }

    public override string part1(string input) {
        return getResult(input, 1, 100000, 1);
    }
   
    public override string part2(string input) {
        return getResult(input, 811589153, 100000000000000, 10);
    }
}