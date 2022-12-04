using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]      
class Solution : Solver {

    public object PartOne(string input) {
        var elvenPairs = input.Split("\n");

        return elvenPairs.Select(p => p.Split(","))
            .Select(s => CreateElfPairChore(s))
            .Where(x => CheckRangeFullyContained(x))
            .Count();
    }

    public object PartTwo(string input) {
        var elvenPairs = input.Split("\n");

        return elvenPairs.Select(p => p.Split(","))
            .Select(s => CreateElfPairChore(s))
            .Where(x => CheckRangePartiallyContained(x))
            .Count();
    }

    private ElfPair CreateElfPairChore(string[] pairs)
    {
        var pair1 = Array.ConvertAll(pairs[0].Split("-"), int.Parse);
        var pair2 = Array.ConvertAll(pairs[1].Split("-"), int.Parse);

        var s1 = Math.Abs(pair1[0] - pair1[1]) + 1;
        var s2 = Math.Abs(pair2[0] - pair2[1]) + 1;

        return s1 >= s2 ?
        new ElfPair (pair1[0], pair1[1], pair2[0], pair2[1]) :
        new ElfPair (pair2[0], pair2[1], pair1[0], pair1[1]);
    }

    private bool CheckRangeFullyContained(ElfPair elfPair)
    {
        return elfPair.startY >= elfPair.startX && elfPair.endY <= elfPair.endX;
    }
    private bool CheckRangePartiallyContained(ElfPair elfPair)
    {
        return elfPair.startY >= elfPair.startX && elfPair.startY <= elfPair.endX ||
               elfPair.endY >= elfPair.startX && elfPair.endY <= elfPair.endX;
    }

    private record ElfPair (int startX, int endX, int startY, int endY);
}
