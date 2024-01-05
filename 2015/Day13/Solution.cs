using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day13;

[ProblemName("Knights of the Dinner Table")]      
class Solution : Solver
{
    private Dictionary<(string, string), int> HMap = new();
    private HashSet<string> Guests = new();
    public object PartOne(string input) {
        ReadInput(input);
        return GetHappinessIndex();
    }

    int GetHappinessIndex() => Guests.GetPermutations(Guests.Count).Select(arrangement =>
        arrangement.Zip(arrangement.Skip(1).Append(arrangement.First()),
                (g1, g2) => HMap.GetValueOrDefault((g1, g2), 0) + HMap.GetValueOrDefault((g2, g1), 0))
            .Sum()).Max();

    public object PartTwo(string input)
    {
        Guests.Add("Harsha");
        return GetHappinessIndex();
    }

    void ReadInput(string input)
    {
            HMap = input.Split('\n').Select(line =>
            {
                var match = Regex.Match(line,
                    @"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+).");
                var arr = match.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray();
                var (t1, t2) = (arr[0], arr[^1]);
                var points = arr[1] == "gain" ? int.Parse(arr[2]) : -1 * int.Parse(arr[2]);
                Guests.Add(arr[0]);
                Guests.Add(arr[^1]);
                return (key: (t1, t2), points);
            }).ToDictionary(kv => kv.key, kv => kv.points);
    }
}
