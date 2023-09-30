using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day10;

[ProblemName("Cathode-Ray Tube")]      
class Solution : Solver {
    List<OpCode> opcodes;
    //int regX = 1;

    record OpCode (string instr, int value);

    public object PartOne(string input) {
        ReadInput(input);

        var samplings = new[] { 20, 60, 100, 140, 180, 220 };
        return ExcecuteCommand()
            .Where(signal => samplings.Contains(signal.ticks))
            .Select(signal => signal.regX * signal.ticks)
            .Sum();
    }

    public object PartTwo(string input) {
        return 0;
    }

    private IEnumerable<(int ticks, int regX)> ExcecuteCommand()
    {
        var (ticks, regX) = (1, 1);

        foreach(var cmd in opcodes)
        {
            switch (cmd.instr)
            {
                case "noop":
                    yield return (ticks++, regX);
                    break;
                case "addx":
                    yield return (ticks++, regX);
                    yield return (ticks++, regX);
                    regX = regX + cmd.value;
                    break;
                default:
                    throw new ArgumentException(cmd.instr + " " + cmd.value);
            }
        }
    }

    private void ReadInput(string input)
    {
        var instructions = input.Split('\n')
            .Select(m => m.Split(' '))
            .Select(l =>
                new OpCode(instr: l[0],
                            value: l.Length > 1 ? Int32.Parse(l[1]) : 0));
        opcodes = new List<OpCode>(instructions);
    }
}
