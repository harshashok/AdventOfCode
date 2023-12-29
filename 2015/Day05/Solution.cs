using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day05;

[ProblemName("Doesn't He Have Intern-Elves For This?")]      
class Solution : Solver
{
    private readonly HashSet<char> vowels = new() { 'a', 'e', 'i', 'o', 'u' };
    readonly HashSet<string> disallowedStrings = new() {"ab", "cd", "pq", "xy"};
    public object PartOne(string input)
    {
        return input
            .Split('\n')
            .Count(line => IsNice(line));
    }

    public object PartTwo(string input) =>
        input.Split('\n')
            .Where(line =>
            {
                var charBetween = Enumerable.Range(0, line.Length - 2).Any(i => line[i] == line[i + 2]);
                var repeatedStr = Enumerable.Range(0, line.Length - 1)
                    .Any(i => line.IndexOf(line.Substring(i, 2), i + 2, StringComparison.Ordinal) >= 0);
                return charBetween && repeatedStr;
            })
            .Count();

    bool IsNice(string line)
    {
        int vowelCnt = 0;
        bool dupChars = false;
        bool disallowedStr = false;
        for (int i = 0; i < line.Length; i++)
        {
            if (vowels.Contains(line[i])) vowelCnt++;

            if (i < line.Length - 1)
            {
                if (line[i] == line[i + 1]) dupChars = true;
                if (disallowedStrings.Contains("" + line[i] + line[i + 1])) disallowedStr = true;
            }
        }

        return (vowelCnt > 2) && dupChars && !disallowedStr;
    }
}
