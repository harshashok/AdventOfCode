using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day10;

[ProblemName("Elves Look, Elves Say")]
class Solution : Solver
{

    public object PartOne(string input) => Transform(input).Take(40).Last().Length;

    public object PartTwo(string input) => Transform(input).Take(50).Last().Length;

    IEnumerable<string> Transform(string input)
    {
        while (true)
        {
            StringBuilder sb = new();
            int i = 0;   //global iterator 
            while (i < input.Length)
            {
                int j = i;  //local counting iterator
                while (j < input.Length && input[i] == input[j])
                {
                    j++;
                }
                sb.Append((j - i).ToString() + input[i]);
                i = j;
            }
            input = sb.ToString();
            yield return input;
        }
    }
}
