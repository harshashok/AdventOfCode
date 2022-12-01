using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]      
class Solution : Solver {
    Dictionary<int, int> digitCountMap = new Dictionary<int, int>()
        {
            {0,6},
            {1,2},
            {2,5},
            {3,5},
            {4,4},
            {5,5},
            {6,6},
            {7,3},
            {8,7},
            {9,6}
        };
    HashSet<int> lengths = new HashSet<int> { 2, 3, 4, 7 };

    /*
              0:      1:      2:      3:      4:      5:      6:      7:      8:      9:
             aaaa    ....    aaaa    aaaa    ....    aaaa    aaaa    aaaa    aaaa    aaaa
            b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
            b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
             ....    ....    dddd    dddd    dddd    dddd    dddd    ....    dddd    dddd
            e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
            e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
             gggg    ....    gggg    gggg    ....    gggg    gggg    ....    gggg    gggg
    */

    public object PartOne(string input) {
        List<string> lines = input.Split('\n').ToList();
        List<string[]> entries = lines.Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries)).ToList();

        return entries.Select(x => x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries))
                      .SelectMany(x => x)
                      .Aggregate(0, (count, str) => count += (lengths.Contains(str.Length)) ? 1 : 0);
    }

    public object PartTwo(string input) {
        List<string> lines = input.Split('\n').ToList();
        List<string[]> entries = lines.Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries)).ToList();

        var entry = entries.First();

        var signals = entry[0].Split(' ');
        var knownSigs = signals.Where(x => lengths.Contains(x.Length));
        var signal5Length = signals.Where(x => x.Length == 5);
        var signal6Length = signals.Where(x => x.Length == 6);

        Dictionary<string, string> resultMapping = new Dictionary<string, string>();

        foreach(string s in knownSigs)
        {
            if (s.Length == 2) resultMapping.Add(s, "1");
            if (s.Length == 3) resultMapping.Add(s, "7");
            if (s.Length == 4) resultMapping.Add(s, "4");
            if (s.Length == 7) resultMapping.Add(s, "8");
        }

        var kpattern = knownSigs.Where(x => x.Length != 7);
        var setKpattern = string.Join("", kpattern).Distinct().ToHashSet();
        var digitPattern1 = knownSigs.Where(x => x.Length == 2).First();
        var digitPattern4 = knownSigs.Where(x => x.Length == 4).First();

        var digitPattern9 = signal6Length.Where(x => x.ToHashSet().Except(setKpattern).Count() == 1).First();
        resultMapping.Add(digitPattern9, "9");

        var digitPattern6 = signal6Length.Where(x => x.ToHashSet().Except(digitPattern9.ToHashSet()).Count() == 1).First();
        resultMapping.Add(digitPattern6, "6");

        var digitPattern0 = signal6Length.Where(x => !x.ToHashSet().SetEquals(digitPattern6) && !x.ToHashSet().SetEquals(digitPattern9)).First();
        resultMapping.Add(digitPattern0, "0");

        var digitPattern5 = signal5Length.Where(x => x.ToHashSet().Intersect(digitPattern4.ToHashSet()).Count() == 3);
        resultMapping.Add(digitPattern5.First(), "5");

        var digitPattern3 = signal5Length.Where(x => x.ToHashSet().Intersect(digitPattern1.ToHashSet()).Count() == 2).First();
        resultMapping.Add(digitPattern3, "3");

        var digitPattern2 = signal5Length.Where(x => !resultMapping.ContainsKey(x)).First();
        resultMapping.Add(digitPattern2, "2");

        return 0;
    }
}
