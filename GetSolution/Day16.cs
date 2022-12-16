namespace Days;

using System.Collections;
using DaySetup;

public class Day16 : Day
{

    private int getShortestPath(Dictionary<string, (int _, HashSet<string> paths)> mapping, string a, string b) {
        Queue<(string p, int l)> q = new Queue<(string p, int l)>();
        q.Enqueue((a, 0));

        while(q.Count > 0) {
            (string p, int l) start = q.Dequeue();           
            if (mapping[start.p].paths.Contains(b)) return start.l + 1;
            foreach(string end in mapping[start.p].paths) q.Enqueue((end, start.l + 1));
        }
        return -1;
    }

    private Dictionary<string, (int flow, (string dest, int length)[] paths)> parseInput(string input) {
        Dictionary<string, (int flow, HashSet<string> paths)> mapping = new Dictionary<string, (int flow, HashSet<string> paths)>();

        foreach(string line in input.Split("\n")) {
            string[] lineSplit = line.Split(" ");
            string key = lineSplit[1];
            int rate = int.Parse(lineSplit[4].Replace("rate=", "").Replace(";", ""));
            string[] paths = lineSplit[8] == "valve"?
                new string[] {lineSplit[9]} : line.Split(" valves ")[1].Split(", ");
            mapping.Add(key, (rate, paths.ToHashSet()));
        }

        Dictionary<string, (int flow, (string dest, int length)[] paths)> adjency = new Dictionary<string, (int flow, (string dest, int length)[] paths)>();

        foreach(string key in mapping.Keys) {
            if(mapping[key].flow == 0 && key != "AA") continue;

            List<(string dest, int length)> adj = new List<(string dest, int length)>();
            foreach(string key2 in mapping.Keys) {
                if (mapping[key2].flow == 0) continue;
                adj.Add((key2, getShortestPath(mapping, key, key2)));
            }
            adjency[key] = (mapping[key].flow, adj.ToArray());
        }
        
        return adjency;
    }

    private int getMaxFlowRate(Dictionary<string, (int flow, (string dest, int length)[] paths)> mapping, string start, int minutes) {
        Queue<(string next, int flow, int minutesLeft, HashSet<string> open)> todo = 
                    new Queue<(string next, int flow, int minutesLeft, HashSet<string> open)>();
        
        todo.Enqueue(("AA", 0, minutes, new HashSet<string>()));
        int maxFlow = 0;

        while (todo.Count > 0) {
            (string next, int flow, int minutesLeft, HashSet<string> open) item = todo.Dequeue();
            (int rate, (string dest, int length)[] paths) info = mapping[item.next];

            item.open.Add(item.next);
            maxFlow = Math.Max(maxFlow, item.flow);

            foreach((string dest, int length) path in info.paths) {
                if(!item.open.Contains(path.dest) && item.minutesLeft - path.length > 1 && mapping.Keys.Contains(path.dest))
                    todo.Enqueue((
                        path.dest, 
                        item.flow + mapping[path.dest].flow * (item.minutesLeft - path.length - 1), 
                        item.minutesLeft - 1 - path.length,
                        new HashSet<string>(item.open)
                    ));
            }
        }
        return maxFlow;
    }


    public override string part1(string input) {
        return getMaxFlowRate(parseInput(input), "AA", 30).ToString();
    }

    public static IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source)
    {
        if (!source.Any())
            return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

        var element = source.Take(1);
        var haveNots = SubSetsOf(source.Skip(1));
        var haves = haveNots.Select(set => element.Concat(set));

        return haves.Concat(haveNots);
    }

    public override string part2(string input) {
        Dictionary<string, (int flow, (string dest, int length)[] paths)> mapping = parseInput(input);

        List<string> keys =  mapping.Keys.ToList();
        keys.Remove("AA");

        int maxFlow = 0;

        foreach(var strings in SubSetsOf(keys)) {
            HashSet<string> set = strings.ToHashSet();

            Dictionary<string, (int flow, (string dest, int length)[] paths)> mappingYou = new Dictionary<string, (int flow, (string dest, int length)[] paths)>();
            Dictionary<string, (int flow, (string dest, int length)[] paths)> mappingEl = new Dictionary<string, (int flow, (string dest, int length)[] paths)>();

            mappingYou["AA"] = mapping["AA"];
            mappingEl["AA"] = mapping["AA"];

            foreach (string key in keys) {
                if (set.Contains(key)) mappingYou[key] = mapping[key];
                else mappingEl[key] = mapping[key];
            }

            maxFlow = Math.Max(maxFlow, getMaxFlowRate(mappingYou, "AA", 26) + getMaxFlowRate(mappingEl, "AA", 26));
        }

        return maxFlow.ToString();
    }
}