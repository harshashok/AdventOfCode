using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day16;

[ProblemName("Aunt Sue")]      
class Solution : Solver
{
    private List<Sue> AuntSues = new();
    record Sue(int number, Dictionary<string, int> map);
    private Dictionary<string, int> ticker = new()
    {
        {"children", 3},
        {"cats", 7},
        {"samoyeds", 2},
        {"pomeranians", 3},
        {"akitas", 0},
        {"vizslas", 0},
        {"goldfish", 5},
        {"trees", 3},
        {"cars", 2},
        {"perfumes", 1}
    };
    
    public object PartOne(string input) {
        ReadInput(input);
        return AuntSues.Select(x => (count: x.map.Intersect(ticker).Count(), number: x.number))
            .MaxBy(kv => kv.count).number;
    }
    public object PartTwo(string input) => AuntSues.Single(sue => CompareAunt(sue.map)).number;

    bool CompareAunt(Dictionary<string, int> map) =>
        map.Keys.All(x =>
        {
            if (x == "cats" || x == "trees")
            {
                return map[x] > ticker[x];
            }
            else if (x == "pomeranians" || x == "goldfish")
            {
                return map[x] < ticker[x];
            }
            else
            {
                return map[x] == ticker[x];
            }
        });

    void ReadInput(string input)
    {
        input.Split('\n')
            .ToList()
            .ForEach(line =>
            {
                var mArr = (from m in Regex.Matches(line, @"(\w+)") select m.Value).ToArray();
                int sueNumber = int.Parse(mArr[1]);
                Dictionary<string, int> sueMap = new();
                for (int i = 2; i < mArr.Length; i += 2)
                {
                    sueMap.Add(mArr[i], int.Parse(mArr[i + 1]));
                }
                AuntSues.Add(new Sue(sueNumber, sueMap));
            });
    }
}
