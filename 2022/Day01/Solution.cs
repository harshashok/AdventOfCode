using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]      
class Solution : Solver {

    IEnumerable<string[]> chunks;

    public object PartOne(string input) => ElfCaloriesInOrder(input).First();

    public object PartTwo(string input) => ElfCaloriesInOrder(input).Take(3).Sum();

    private IEnumerable<int> ElfCaloriesInOrder(string input)
    {
        chunks = input.Split("\n\n").Select(x => x.Split("\n"));
        return chunks.Select(arr => Array.ConvertAll(arr, int.Parse))
        .Select(x => x.Sum()).OrderDescending();        
    }
}
