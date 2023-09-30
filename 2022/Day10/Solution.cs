using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2022.Day10;

[ProblemName("Cathode-Ray Tube")]      
class Solution : Solver {
    List<OpCode> opcodes;
    //int regX = 1;

    record OpCode (string instr, int value);

    public object PartOne(string input) {
        ReadInput(input);

        var samplings = new[] { 20, 60, 100, 140, 180, 220 };
        return ExecuteCommand()
            .Where(signal => samplings.Contains(signal.ticks))
            .Select(signal => signal.regX * signal.ticks)
            .Sum();
    }

    public object PartTwo(string input) {
        var crtLine = 0;
        return ExecuteCommand()
            .Select(item =>
            {
                var cycle = (item.ticks) - (40 * crtLine);
                var x = item.regX;
                if (item.ticks % 40 == 0) { crtLine++; }
                return ((cycle == x) || (cycle == x + 1) || (cycle == x + 2)) ? '#' : ' ';
            })
            .Chunk(40)
            .Select(line => new string(line))
            .Aggregate("", (screen, line) => screen + line + "\n")
            .Ocr();
    }

    private object solvePart2Iterator()
    {
        Console.WriteLine();

        var iterator = ExecuteCommand().GetEnumerator();
        var samplings = new[] { 40, 80, 120, 160, 200, 240 };
        int crtLine = 0;

        while (iterator.MoveNext())
        {
            var item = iterator.Current;
            var cycle = item.ticks - (40 * crtLine);
            if ((cycle == item.regX) || (cycle == item.regX + 1) || (cycle == item.regX + 2))
            {
                Render("#");
            }
            else
            {
                Render(" ");
            }

            if (samplings.Contains(item.ticks))
            {
                Console.WriteLine();
                crtLine++;
            }
        }
        Console.WriteLine();
        return 0;
    }

    private IEnumerable<(int ticks, int regX)> ExecuteCommand()
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

    private void Render(string text)
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = (text == ".") ? ConsoleColor.DarkGray : ConsoleColor.DarkGreen;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}
