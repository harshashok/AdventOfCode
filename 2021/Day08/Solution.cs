using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]      
class Solution : Solver {

    public object PartOne(string input) {
        List<string> lines = input.Split('\n').ToList();
        List<string[]> readings = lines.Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries)).ToList();
        HashSet<int> lengths = new HashSet<int> { 2, 3, 4, 7 };

        return readings.Select(x => x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                       .SelectMany(x => x)
                       .Aggregate(0, (count, str) => count += (lengths.Contains(str.Length)) ? 1 : 0);
    }

    public object PartTwo(string input) {
        return 0;
    }
}
