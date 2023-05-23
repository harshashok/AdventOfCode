using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]      
class Solution : Solver {

    Point head;
    Point tail;
    IEnumerable<Move> moves;
    HashSet<Point> visited;
    Point[] knots;
    private record Move(string dir, int steps);

    public object PartOne(string input) {
        ReadInput(input);
        SetupVars(2);
        SimulateMotion();
        return visited.Count;
    }

    public object PartTwo(string input) {
        SetupVars(10);
        SimulateMotion();
        return visited.Count;
    }

    private void SimulateMotion()
    {
        foreach (var move in moves)
        {
            SimulateSingleMoveSteps(move);
        }
    }

    private void SimulateSingleMoveSteps(Move move)
    {
        for (int i = 0; i < move.steps; i++)
        {
            MoveInDirection(move.dir, ref knots[0]);
            for (int k = 0; k < knots.Length - 1; k++)
            {
                CalculateOffsetV2(ref knots[k], ref knots[k + 1]);
                visited.Add(new Point(knots[^1].X, knots[^1].Y));
            }
        }
    }

    private void MoveInDirection(string dir, ref Point node)
    {
        node = dir switch
        {
            "U" => new Point(node.X + 1, node.Y),
            "D" => new Point(node.X - 1, node.Y),
            "L" => new Point(node.X, node.Y - 1),
            "R" => new Point(node.X, node.Y + 1),
            _ => throw new ArgumentOutOfRangeException(dir)
        };
    }

    /* Offset calc
    *   (x+2,y-2)  (x+2,y-1)         (x+2,y+1)  (x+2,y+2)
    *   (x+1,y-2) | x+1,y-1 | x+1,y | x+1,y+1 | (x+1,y+2)
    *             |  x,y-1  | x,y   |  x,y+1  |
    *   (x-1,y-2) | x-1,y-1 | x-1,y | x-1,y+1 | (x-1,y+2)   
    *   (x-2,y-2)  (x-2,y-1)         (x-2,y+1)  (x-2,y+2)
    */

    private void CalculateOffsetV2(ref Point head, ref Point tail)
    {
        var offsetX = tail.X - head.X;
        var offsetY = tail.Y - head.Y;

        if(Math.Abs(offsetX) > 1 || Math.Abs(offsetY) > 1)
        {
            tail = new Point(tail.X - Math.Sign(offsetX), tail.Y - Math.Sign(offsetY));
        }
    }

    private void ReadInput(string input)
    {
        moves = input.Split('\n')
            .Select(m => m.Split(' '))
            .Select(l => new Move(dir: l[0], steps: Int32.Parse(l[1])));
    }

    private void SetupVars(int knotCount)
    {
        knots = Enumerable.Repeat(new Point(0, 0), knotCount).ToArray();
        visited = new();
        visited.Add(new Point(0, 0));
    }
}