using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2024.Day07;

[ProblemName("Bridge Repair")]      
class Solution : Solver
{
    private IEnumerable<Calibration> calibrations;

    public object PartOne(string input) {
        ReadInput(input);
        return calibrations.Where(cal => cal.IsValidCalibration(false))
            .Sum(eq => eq.target);
    }
    

    public object PartTwo(string input) =>
        calibrations.Where(cal => cal.IsValidCalibration(true))
            .Sum(eq => eq.target);

    private record Calibration(long[] nums, long target)
    {
        public bool IsValidCalibration(bool useConcat) => IsValidCalibration(0, 0, useConcat);
        
        private bool IsValidCalibration(long sum, int index, bool useConcat)
        {
            if (index == nums.Length) return sum == target;
            if (sum > target) return false;

            return IsValidCalibration(sum + nums[index], index + 1, useConcat) ||
                   IsValidCalibration(Math.Max(sum, 1) * nums[index], index + 1, useConcat) ||
                   (useConcat && IsValidCalibration(Concatenate(sum, nums[index]), index + 1, true));
        }
        
        private static long Concatenate(long first, long second) => long.Parse(first.ToString() + second.ToString());
    }

    private void ReadInput(string input) =>
        calibrations = input.Split('\n')
            .Select(line => line.Split(':'))
            .Select(r =>
            {
                long total = long.Parse(r[0]);
                long[] nums = Array.ConvertAll(r[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse);
                return new Calibration(nums, total);
            });
}
