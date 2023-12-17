using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]      
class Solution : Solver
{
    private Dictionary<string, List<Map>> maps = new();
    private IEnumerable<long> seeds;
    public object PartOne(string input) {
        ReadInput(input);
        return seeds.Select(x => ReMap(maps["seed-to-soil"], x))
            .Select(x => ReMap(maps["soil-to-fertilizer"], x))
            .Select(x => ReMap(maps["fertilizer-to-water"], x))
            .Select(x => ReMap(maps["water-to-light"], x))
            .Select(x => ReMap(maps["light-to-temperature"], x))
            .Select(x => ReMap(maps["temperature-to-humidity"], x))
            .Select(x => ReMap(maps["humidity-to-location"], x))
            .Min();
    }

    public object PartTwo(string input) {
        return 0;
    }

    long ReMap(List<Map> maps, long source)
    {
        foreach (var map in maps)
        {
            if (source >= map.srcStart && source < map.srcStart + map.length)
                return map.destStart + (source - map.srcStart);
        }

        return source;
    }
    void ReadInput(string input)
    {
        var blocks = input.Split("\n\n");
        seeds = from m in Regex.Matches(blocks[0], @"\d+") select long.Parse(m.Value);
        maps = blocks.Skip(1).Select(ParseMap).ToDictionary(x => x.Key, x => x.Value);
    }

    KeyValuePair<string, List<Map>> ParseMap(string input)
    {
        var block = input.Split('\n');
        var mapType = block.First();

        var list = from line in block.Skip(1)
                    let values = Array.ConvertAll(line.Split(), long.Parse)
                    select new Map(values[0], values[1], values[2]);
        return new KeyValuePair<string, List<Map>>(mapType.Split()[0], list.ToList());
    }
    
    record Map(long destStart, long srcStart, long length);
}
