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
            .ToList()
            .Select(p => (p[0] * 10) + p[^1])
            .Sum();
    }

    public object PartTwo(string input) {
        return 0;
    }
}
