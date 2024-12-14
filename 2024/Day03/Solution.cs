using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day03;

[ProblemName("Mull It Over")]      
class Solution : Solver {

    public object PartOne(string input) => ReadInput(input, new Regex(@"mul\((\d+),(\d+)\)")).Select(pair => pair.first * pair.second).Sum();

    public object PartTwo(string input) => 
        ReadInput(input, new Regex(@"(?<=(?:do\(\)|^)(?:[^d]|d(?!on't\(\)))*)mul\((\d{1,3}),(\d{1,3})\)")).Select(pair => pair.first * pair.second).Sum();

    private IEnumerable<(int first, int second)> ReadInput(string input, Regex regex) =>
        regex.Matches(input)
            .Select(m =>
                m.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray())
            .Select(s => (int.Parse(s[0]), int.Parse(s[1])));
    
    //Alt query linq.
    private IEnumerable<(int first, int second)> ReadInput2(string input, Regex regex)
    {
       var qq = from m in regex.Matches(input)
            let arr = m.Groups.Cast<Group>().Skip(1).Select(x => x.Value).ToArray()
            select (int.Parse(arr[0]), int.Parse(arr[1]));
       return qq;
    }
}
