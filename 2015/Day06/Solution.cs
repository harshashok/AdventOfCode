using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day06;

[ProblemName("Probably a Fire Hazard")]      
class Solution : Solver
{
    private int[,] grid = new int[1000, 1000];
    public object PartOne(string input) {
        RunLightInstructions(input, val => 1, val => 0, val => 1 - val);
        return grid.Cast<int>().Sum();    
    }

    public object PartTwo(string input)
    {
        grid = new int[1000, 1000];
        RunLightInstructions(input, val => val + 1, val => val - 1 > 0 ? val-1 : 0, val => val + 2);
        return grid.Cast<int>().Sum();
    }

    void RunLightInstructions(string input, Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle)
    {
        input.Split('\n')
            .ToList()
            .ForEach(line =>
            {
                var nums = (from m in Regex.Matches(line, @"\d+") select int.Parse(m.Value)).ToArray();

                if (line.StartsWith("turn on")) 
                    ParseInstruction(nums[0], nums[1], nums[2], nums[3], turnOn);
                else if (line.StartsWith("turn off"))
                    ParseInstruction(nums[0], nums[1], nums[2], nums[3], turnOff);
                else 
                    ParseInstruction(nums[0], nums[1], nums[2], nums[3], toggle);
            });
    }
    
    void ParseInstruction(int startX, int startY, int endX, int endY, Func<int, int> lightAction)
    {
        for (var row = startX; row <= endX; row++)
        {
            for (int col = startY; col <= endY; col++)
            {
                grid[row, col] = lightAction(grid[row, col]);
            }
        }
    }
}
