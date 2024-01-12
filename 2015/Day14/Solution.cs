using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Common;

namespace AdventOfCode.Y2015.Day14;

[ProblemName("Reindeer Olympics")]      
class Solution : Solver
{
    private List<Reindeer> ReindeerList = new();
    record Reindeer(string name, int speed, int flytime, int rest);
    
    public object PartOne(string input) {
        ReadInput(input);
        return ComputeRace(2503).Max();
    }

    public object PartTwo(string input)
    {
        int rCount = ReindeerList.Count;
        int[] points = new int[rCount];
        
        for (int cycle = 1; cycle <= 2503; cycle++)
        {
            var raceTimes = ComputeRace(cycle);
            var max = raceTimes.Max();
            for (int i = 0; i < rCount; i++)
            {
                if (raceTimes[i] == max)
                {
                    points[i]++;
                }
            }
        }
        return points.Max();
    }

    int[] ComputeRace(int time)
    {
        return ReindeerList.Select(r => DistanceTravelled(r, time)).ToArray();
    }   
    /**
     *  The first part is the distance traveled during one cycle times the number of full cycles that a reindeer will fly and then rest.
     * v*g is the distance traveled in one cycle and t/(g + r) is the total number of completed cycles (int rounds down all division of integers,
     * it should really be floor(t/(g+r)).
     * So that takes care of all of the full cycles. Now what about the remainder if t isn't divisible by g+r?
     * t % (g + r) is the remaining time after all of the full cycles are completed. Since the remaining time could be greater than g,
     * take min(g, t % (g+r)). If g is smaller than t % (g+r) then the reindeer travels for its maximum time during the remainder.
     * If g is greater then the reindeer travels for t % (g+r) during the remainder.
     */
    int DistanceTravelled(Reindeer r, int time) =>
        r.speed * r.flytime * (time / (r.flytime + r.rest)) +
        r.speed * Math.Min(r.flytime, time % (r.flytime + r.rest));
    
    void ReadInput(string input)
    {
        input.Split('\n')
            .ToList()
            .ForEach(line =>
            {
                var match = Regex.Match(line,
                    @"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.");
                var arr = match.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray();
                ReindeerList.Add(new Reindeer(arr[0], int.Parse(arr[1]), int.Parse(arr[2]), int.Parse(arr[3])));
            });
    }
}
