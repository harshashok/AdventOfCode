using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day02;

[ProblemName("I Was Told There Would Be No Math")]      
class Solution : Solver
{
    private IEnumerable<Present> presents;
    public object PartOne(string input)
    {
        ReadInput(input);
        return presents.Select(p =>
            (2 * p.length * p.width) + (2 * p.width * p.height) + (2 * p.height * p.length) + p.length * p.width).Sum();
    }

    public object PartTwo(string input) => presents.Select(p => ((2 * p.length) + (2 * p.width)) + (p.length * p.width * p.height)).Sum();

    void ReadInput(string input) =>
        presents = input.Split('\n')
            .Select(line => line.Split('x'))
            .Select(nums => Array.ConvertAll(nums, int.Parse))
            .Select(nums => nums.OrderBy(x => x).ToArray())
            .Select(nums => new Present(nums[0], nums[1], nums[2]));

    record Present(int length, int width, int height);
}
