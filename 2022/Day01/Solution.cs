using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]      
class Solution : Solver {

    IEnumerable<string[]> chunks;

    public object PartOne(string input) {
        return AggregateSum(input).Max();
    }

    public object PartTwo(string input) {
        return AggregateSum(input)
            .OrderDescending()
            .Take(3).Sum();
    }

    private IEnumerable<int> AggregateSum(string input)
    {
        chunks = input.Split("\n\n").Select(x => x.Split("\n"));
        return chunks.Select(arr => Array.ConvertAll(arr, int.Parse))
        .Select(x => x.Sum());
        
    }
}
