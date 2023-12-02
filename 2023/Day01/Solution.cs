using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]      
class Solution : Solver {

    public object PartOne(string input) {
        Regex regex = new(@"-?[0-9]");

        return input.Split('\n')
            .Select(line => regex.Matches(line))
            .Select(m => m.Select(m => m.Value))
            .Select(p => Array.ConvertAll(p.ToArray(), int.Parse))
            .Select(p => (p[0] * 10) + p[^1])
            .Sum();
    }

    public object PartTwo(string input) {
        Regex regex = new("(?=([0-9]|one|two|three|four|five|six|seven|eight|nine|zero))");

        return input.Split('\n')
            .Select(line => regex.Matches(line)).ToList()
            .Select(m => m.SelectMany(m => m.Groups.Cast<Group>().Skip(1).Select(g => g.Value)))
            .Select(p => Convert(p.ToArray()))
            .Sum();
    }

    int Convert(string[] p) => (numMap.TryGetValue(p[0], out int value) ? value : int.Parse(p[0])) * 10 +
                               (numMap.TryGetValue(p[^1], out int value2) ? value2 : int.Parse(p[^1]));

    Dictionary<string, int> numMap = new()
    {
        {"one", 1 },
        {"two", 2 },
        {"three", 3 },
        {"four", 4 },
        {"five", 5 },
        {"six", 6 },
        {"seven", 7 },
        {"eight", 8 },
        {"nine", 9 },
        {"zero", 0 }
    };
}
