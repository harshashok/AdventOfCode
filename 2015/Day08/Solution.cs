using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day08;

[ProblemName("Matchsticks")]      
class Solution : Solver {

    public object PartOne(string input) =>
        input.Split('\n')
            .Select(line => line.Length -  Regex.Unescape(line.Substring(1, line.Length - 2)).Length)
            .Sum();

    public object PartTwo(string input) =>
        input.Split('\n')
            .Select(line => (line.Replace("\\", "\\\\").Replace("\"", "\\\"").Length + 2) - line.Length)
            .Sum();
}
