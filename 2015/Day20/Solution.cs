using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day20;

[ProblemName("Infinite Elves and Infinite Houses")]      
class Solution : Solver {

    public object PartOne(string input) => CalcPresents(houses: 1000000, target: Int32.Parse(input), multiplier: 10);

    public object PartTwo(string input) {
        return 0;
    }

    private int CalcPresents(int houses, int target, int multiplier)
    {
        long[] presents = new long [houses+1];

        for (int elf = 1; elf <= houses; elf++)
        {
            int skipCount = elf;
            for (int house = elf; house <= houses;)
            {
                if (house % elf == 0)
                {
                    presents[house] += elf * multiplier;
                }
                house += skipCount;
            }
        }


        for (int i = 1; i < presents.Length; i++)
        {
            if (presents[i] >= target)
                return i;
        }

        return -1;
    }
}
