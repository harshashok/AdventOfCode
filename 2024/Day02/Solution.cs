using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day02;

[ProblemName("Red-Nosed Reports")]      
class Solution : Solver
{
    public object PartOne(string input) => ReadInput(input).Count(IsReportSafe);

    public object PartTwo(string input) => ReadInput(input).Count(line => GenerateSequences(line).Any(IsReportSafe));

    private bool IsReportSafe(int[] arr)
    {
        int increasing = 0;
        int decreasing = 0;
        
        for (int i = 0; i < arr.Length-1; i++)
        {
            if (arr[i] > arr[i + 1] && Math.Abs(arr[i] - arr[i + 1]) <= 3) increasing++;
            else if (arr[i] < arr[i + 1] && Math.Abs(arr[i] - arr[i + 1]) <= 3) decreasing++;
            else return false;
        }

        if (increasing == arr.Length - 1 || decreasing == arr.Length - 1) return true;
        
        return false;
    }
    
    private IEnumerable<int[]> GenerateSequences(int[] arr) =>
        Enumerable.Range(0, arr.Length + 1)
            .Select(i =>
            {
                var first = arr.Take(i - 1);
                var second = arr.Skip(i);
                return Enumerable.Concat(first, second).ToArray();
            });

    private IEnumerable<int[]> ReadInput(string input) => input.Split('\n').Select(line => line.Split(' ').Select(int.Parse).ToArray());
}
