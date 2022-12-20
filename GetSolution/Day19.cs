namespace Days;

using DaySetup;

public class Day19 : Day
{  

    private int getMaxObsidian(
        int minutesLeft, int ore, int clay, int obs, int geode,
        int oreRobotCount, int clayRobotCount,int obsRobotCount, int geoRobotCount, 
        int oreRobotCost, int clayRobotCost, (int ore, int clay) obsRobotCost, (int ore, int obs) geoRobotCost,
        HashSet<(int minutesLeft, int ore, int clay, int obs, int geode, int oreRobotCount, int clayRobotCount, int obsRobotCount, int geoRobotCount)> memory
    ) {
        int maxGeode = geode;

        if(memory.Contains((
            minutesLeft, ore, clay, obs, geode, oreRobotCount, clayRobotCount, obsRobotCount, geoRobotCount
        ))) return -1;

        memory.Add((
            minutesLeft, ore, clay, obs, geode, oreRobotCount, clayRobotCount, obsRobotCount, geoRobotCount
        ));
        
        while(true) {
            if(minutesLeft > 1 && (
                ore >= oreRobotCost || 
                ore >= clayRobotCost || (
                    ore >= obsRobotCost.ore && clay >= obsRobotCost.clay
                ) || (
                    ore >= geoRobotCost.ore && obs >= geoRobotCost.obs
                )
            ) || minutesLeft == 0) break;

            minutesLeft--;
            ore += oreRobotCount;
            clay += clayRobotCount;
            obs += obsRobotCount;
            geode += geoRobotCount;
        }

        if(minutesLeft == 0) 
            return geode;
        
        // Buy geoRobot
        if(minutesLeft > 1 && ore >= geoRobotCost.ore && obs >= geoRobotCost.obs) maxGeode = Math.Max(maxGeode, getMaxObsidian(
            minutesLeft - 1, ore + oreRobotCount - geoRobotCost.ore, clay + clayRobotCount, 
            obs + obsRobotCount - geoRobotCost.obs, geode + geoRobotCount, oreRobotCount, clayRobotCount,
            obsRobotCount, geoRobotCount + 1, oreRobotCost, clayRobotCost, obsRobotCost, geoRobotCost, memory
        ));

        // Buy obsRobot
        if(minutesLeft > 1 && ore >= obsRobotCost.ore && clay >= obsRobotCost.clay) maxGeode = Math.Max(maxGeode, getMaxObsidian(
            minutesLeft - 1, ore + oreRobotCount - obsRobotCost.ore, clay + clayRobotCount - obsRobotCost.clay, 
            obs + obsRobotCount, geode + geoRobotCount, 
            oreRobotCount, clayRobotCount, obsRobotCount + 1, geoRobotCount, oreRobotCost, clayRobotCost, obsRobotCost, geoRobotCost, memory
        ));

        // Buy clayRobot
        if(minutesLeft > 1 && ore >= clayRobotCost) maxGeode = Math.Max(maxGeode, getMaxObsidian(
            minutesLeft - 1, ore + oreRobotCount - clayRobotCost, clay + clayRobotCount, obs + obsRobotCount, 
            geode + geoRobotCount, oreRobotCount, clayRobotCount + 1,
            obsRobotCount, geoRobotCount, oreRobotCost, clayRobotCost, obsRobotCost, geoRobotCost, memory
        ));

        // Buy oreRobot
        if(minutesLeft > 1 && ore >= oreRobotCost) maxGeode = Math.Max(maxGeode, getMaxObsidian(
            minutesLeft - 1, ore - oreRobotCost + oreRobotCount, clay + clayRobotCount, obs + obsRobotCount, 
            geode + geoRobotCount, oreRobotCount + 1, clayRobotCount,
            obsRobotCount, geoRobotCount, oreRobotCost, clayRobotCost, obsRobotCost, geoRobotCost, memory));

        // Buy nothing
        maxGeode = Math.Max(maxGeode, getMaxObsidian(
            minutesLeft - 1, ore + oreRobotCount, clay + clayRobotCount, obs + obsRobotCount, geode + geoRobotCount, 
            oreRobotCount, clayRobotCount, obsRobotCount, geoRobotCount, oreRobotCost, clayRobotCost, obsRobotCost, geoRobotCost, memory
        ));

        return maxGeode;
    }


    private string getAnswer(string input, int minutes, int blueprints, bool part1) {
        var parsed = input.Split("\n").Take(blueprints).Select(line => (
            int.Parse(line.Split(" ")[1].Replace(":", "")),
            int.Parse(line.Split(" ")[6]),
            int.Parse(line.Split(" ")[12]),
            (
                int.Parse(line.Split(" ")[18]),
                int.Parse(line.Split(" ")[21])
            ),
            (
                int.Parse(line.Split(" ")[27]),
                int.Parse(line.Split(" ")[30])
            )
        )).ToArray();

        int sum = 0;
        int prod = 1;

        foreach(var item in parsed) {
            int result = getMaxObsidian(
                minutes, 0, 0, 0, 0, 1, 0, 0, 0, item.Item2, item.Item3, item.Item4, item.Item5, 
                new HashSet<(int minutesLeft, int ore, int clay, int obs, int geode, int oreRobotCount, int clayRobotCount, int obsRobotCount, int geoRobotCount)>()
            );
            if(part1) sum += item.Item1 * result;
            else prod *= result;
        }

        return (part1 ? sum : prod).ToString();
    }
    public override string part1(string input) {
        return getAnswer(input, 24, 30, true);
    }
   
    public override string part2(string input) {
        return getAnswer(input, 32, 3, false);
    }
}