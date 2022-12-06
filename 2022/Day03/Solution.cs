using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]      
class Solution : Solver {

    public object PartOne(string input) {
        var rucksacks = input.Split("\n");

        return rucksacks.Select(x => new Tuple<string, string>(x.Substring(0, x.Length / 2), x.Substring(x.Length / 2, x.Length / 2)))
                        .Select(y => y.Item1.Intersect(y.Item2).Single())
                        .Select(z => itemPriority(z))
                        .Sum();
    }

    public object PartTwo(string input) {
        var rucksacks = input.Split("\n");

        return ElvenGroups(rucksacks, 3)
                    .Select(x => x.ElementAt(0).Intersect(x.ElementAt(1)).Intersect(x.ElementAt(2)).Single())
                    .Select(z => itemPriority(z))
                    .Sum();
    }

    private int itemPriority(char ch)
    {
        return Char.IsLower(ch) ? ((int)ch - 96) : (int)ch - 38;
    }

    // Break a list of items into chunks of a specific size
    private IEnumerable<IEnumerable<T>> ElvenGroups<T>(IEnumerable<T> source, int chunksize)
    {
        while (source.Any())
        {
            yield return source.Take(chunksize);
            source = source.Skip(chunksize);
        }
    }
}
