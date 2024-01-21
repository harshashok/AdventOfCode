using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day18;

[ProblemName("Like a GIF For Your Yard")]      
class Solution : Solver
{
    private char[][] grid;
    private (int row, int col)[] diffs = new (int row, int col)[]
        { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
    private List<(int x, int y, char ch)> StateChange = new();
    
    public object PartOne(string input) {
        ReadInput(input);
        RunLightSteps(100);
        return grid.SelectMany(x => x).Count(q => q == '#');
    }

    public object PartTwo(string input) {
        ReadInput(input);
        RunLightSteps(100, true);
        return grid.SelectMany(x => x).Count(q => q == '#');
    }

    void RunLightSteps(int steps, bool stuck = false)
    {
        for (int i = 0; i < steps; i++) RunSingleStep(stuck);
    }
    
    void RunSingleStep(bool stuck = false)
    {   
        int X = grid.Length-1;
        int Y = grid[0].Length-1;
        if (stuck)
        {
            grid[0][0] = '#';
            grid[0][Y] = '#';
            grid[X][0] = '#';
            grid[X][Y] = '#';
        }
        
        for (int x = 0; x < grid.Length; x++)
        {
            for (int y = 0; y < grid[0].Length; y++)
            {
                if (stuck && ((x == 0 && y == 0) || (x == 0 && y == Y) || (x == X && y == 0) || (x == X && y == Y)))
                {
                    continue;
                }

                var nbors = CalcState(x, y);
                    if (grid[x][y] == '#' && !(nbors is 2 or 3))
                    {
                        StateChange.Add((x, y, '.'));
                    }
                    else 
                    {
                        if (nbors is 3)
                        {
                            StateChange.Add((x, y, '#'));
                        }
                    }
            }
        }
        //Flush
        StateChange.ForEach(s => grid[s.x][s.y] = s.ch);
        StateChange.Clear();
    }
    
    int CalcState(int x, int y) =>
        diffs
            .Select(p => (X: p.row + x, Y: p.col + y))
            .Count(t => t.X >= 0 && t.Y >= 0 && t.X < grid.Length && t.Y < grid[0].Length && grid[t.X][t.Y] == '#');

    void ReadInput(string input)
    {
        grid = input.Split('\n')
            .Select(x => x.Select(y => y).ToArray())
            .ToArray();
    }
}
