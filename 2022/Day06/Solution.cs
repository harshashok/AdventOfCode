using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]      
class Solution : Solver {

    public object PartOne(string input) => ParseBuffer(input, 4);
    public object PartTwo(string input) => ParseBuffer(input, 14);

    private int ParseBuffer(string input, int slidingWindow)
    {
        Queue<char> window = new(slidingWindow);
        var arr = input.ToArray();

        for (int i = 0; i < arr.Length; i++)
        {
            while (window.Contains(arr[i]))
            {
                window.Dequeue();
            }

            window.Enqueue(arr[i]);
            if (window.Count == slidingWindow)
            {
                return i+1;
            }
        }
        return -1;
    }

    private void TestFromEncse(string input, int slidingWindow)
    {
        Enumerable.Range(slidingWindow, input.Length)
           .First(i => input.Substring(i - slidingWindow, slidingWindow).ToHashSet().Count == slidingWindow);
    }
}
