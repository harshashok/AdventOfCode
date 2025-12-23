using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2025.Day01;

[ProblemName("Secret Entrance")]      
class Solution : Solver {

    int dialSetting = 50;
    private IEnumerable<int> rotations;
    
    public object PartOne(string input)
    {
        rotations = input.Split('\n')
            .Select(line =>
            {
                var (dir, value) = (line.Substring(0, 1), int.Parse(line.Substring(1)));
                return (dir == "L" ? -1 : 1) * value;
            });

        return rotations.Count(PerformRotation);
    }
    
    public object PartTwo(string input)
    {
        dialSetting = 50;
        
        return rotations.Select(PerformRotationAndCrossing).Sum();
    }
    
    private bool PerformRotation(int value)
    {
        dialSetting += value % 100;

        if (dialSetting < 0) dialSetting += 100;
        if (dialSetting >= 100) dialSetting -= 100;
        
        //Console.WriteLine($"The dial is rotated {value} to point at {dialSetting}");
        return dialSetting == 0;
    }

    private int PerformRotationAndCrossing(int value)
    {
        bool skipCount = dialSetting == 0;
        int zeroCrossing = Math.Abs(value) / 100;
        
        int remainingTurns = value % 100;
        dialSetting += remainingTurns;
        
        if (dialSetting < 0)
        {
            dialSetting += 100;
            if (!skipCount) zeroCrossing++;
        }

        if (dialSetting > 100)
        {
            dialSetting -= 100;
            if (!skipCount) zeroCrossing++;
        }

        dialSetting = (dialSetting == 100) ? 0 : dialSetting;
        zeroCrossing += (dialSetting == 0) ? 1 : 0;

        //Console.WriteLine($"The dial is rotated {value} to point at {dialSetting} | zeroCrossing : {zeroCrossing}");
        
        int total = zeroCrossing;
        return total;
    }
}
