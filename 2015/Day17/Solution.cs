using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day17;

[ProblemName("No Such Thing as Too Much")]      
class Solution : Solver
{
    public object PartOne(string input) => FillContainerCombination(ReadInput(input), 150).Count();
    
    public object PartTwo(string input)
    {
        var combos = FillContainerCombination(ReadInput(input), 150).ToArray();
        var min = combos.Select(p => p.Count()).Min();
        return combos.Count(a => a.Count() == min);
    }
    
    IEnumerable<IEnumerable<int>> FillContainerCombination(int[] containers, int eggnog) =>
        Enumerable.Range(1, containers.Length + 1)
            .SelectMany(i => containers.GetCombinations(i).Where(c => c.Sum() == eggnog));

    int[] ReadInput(string input) => (from m in input.Split('\n') select int.Parse(m)).ToArray();
}
