using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day01;

[ProblemName("Not Quite Lisp")]      
class Solution : Solver {

    public object PartOne(string input) => input.Aggregate(0, (accum, y) => accum + (y == '('? 1 : -1));

    public object PartTwo(string input) => GetFloor(input).First(x => x.floor == -1).index;

    IEnumerable<(int floor, int index)> GetFloor(string input)
    {
        var floor = 0;
        for (int index = 0; index < input.Length; index++)
        {
            floor += (input[index] == '(' ? 1 : -1);
            yield return (floor, index+1);
        }
    }
    
}
