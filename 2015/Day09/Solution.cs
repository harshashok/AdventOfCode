using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day09;

[ProblemName("All in a Single Night")]      
class Solution : Solver
{
    private Dictionary<Tuple<string, string>, int> distances = new();
    private HashSet<string> towns = new();
    public object PartOne(string input) {
        ReadInput(input);
        return GetDistances().Min();
    }

    public object PartTwo(string input) => GetDistances().Max();
    
    IEnumerable<int> GetDistances() => 
        GetPermutations(towns, towns.Count).Select(path => path.Zip(path.Skip(1), (a, b) => distances[Tuple.Create(a, b)]).Sum());
    
    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });

        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)), 
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }
    
    void ReadInput(string input)
    {
        input.Split('\n')
            .ToList()
            .ForEach(line =>
            {
                var match = Regex.Match(line, @"(\w+) to (\w+) = (\d+)");
                var arr = match.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray();
                distances.Add(new Tuple<string, string>(arr[0], arr[1]), int.Parse(arr[2]));
                distances.Add(new Tuple<string, string>(arr[1], arr[0]), int.Parse(arr[2]));

                towns.Add(arr[0]);
                towns.Add(arr[1]);
            });
    }
}
