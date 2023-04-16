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
    IEnumerable<move> moves;
    HashSet<Point> visited;
    Point[] knots;
    private record move(string dir, int steps);

    enum dir
    {
        U,
        D,
        R,
        L
    }

    public object PartOne(string input) {
        moves = input.Split('\n')
                .Select(m => m.Split(' '))
                .Select(l => new move(dir: l[0], steps: Int32.Parse(l[1])));

        head = new(0, 0);
        tail = new(0, 0);
        visited = new();
        visited.Add(tail);
        SimulateMotion();

        return visited.Count;
    }

    public object PartTwo(string input) {
        return 0;
    }

    private void SimulateMotion()
    {
        foreach (var move in moves)
        {
            SimulateSingleMove(move);
        }
    }

    private void SimulateSingleMove(move move)
    {
        if (move.dir == "U")
        {
            for (int i = 0; i < move.steps; i++)
            {
                head.Offset(1, 0);
                if (!InVicinity(head, tail))
                {
                    CalculateOffSet();
                    visited.Add(new Point(tail.X, tail.Y));
                }
            }
        }
        else if (move.dir == "D")
        {
            for (int i = 0; i < move.steps; i++)
            {
                head.Offset(-1, 0);
                if (!InVicinity(head, tail))
                {
                    CalculateOffSet();
                    visited.Add(new Point(tail.X, tail.Y));
                }
            }
        }
        else if (move.dir == "L")
        {
            for (int i = 0; i < move.steps; i++)
            {
                head.Offset(0, -1);
                if (!InVicinity(head, tail))
                {
                    CalculateOffSet();
                    visited.Add(new Point(tail.X, tail.Y));
                }
            }
        }
        else if (move.dir == "R")
        {
            for (int i = 0; i < move.steps; i++)
            {
                head.Offset(0, 1);
                if (!InVicinity(head, tail))
                {
                    CalculateOffSet();
                    visited.Add(new Point(tail.X, tail.Y));
                }
            }
        }
    }

    /* Offset calc
    *              (x+2,y-1)         (x+2,y+1)
    *   (x+1,y-2) | x+1,y-1 | x+1,y | x+1,y+1 | (x+1,y+2)
    *             |  x,y-1  | x,y   |  x,y+1  |
    *   (x-1,y-2) | x-1,y-1 | x-1,y | x-1,y+1 | (x-1,y+2)   
    *              (x-2,y-1)         (x-2,y+1)
    */
    private void CalculateOffSet()
    {
        if (tail.X == head.X)  //same row
        {
            if (tail.Y > head.Y) tail.Offset(0, -1);
            else tail.Offset(0,1);
        }
        else if (tail.Y == head.Y) //same column
        {
            if (tail.X > head.X) tail.Offset(-1, 0);
            else tail.Offset(1, 0);
        }
        else
        {
            if (tail.X == head.X + 2) tail = new Point(head.X + 1, head.Y);
            if (tail.X == head.X - 2) tail = new Point(head.X - 1, head.Y);
            if (tail.Y == head.Y + 2) tail = new Point(head.X, head.Y + 1);
            if (tail.Y == head.Y - 2) tail = new Point(head.X, head.Y - 1);
        }
    }

    /* Vicinity
    *   | x+1,y-1 | x+1,y | x+1,y+1 |
    *   |  x,y-1  | x,y   |  x,y+1  |
    *   | x-1,y-1 | x-1,y | x-1,y+1 |
    */
    private bool InVicinity(Point head, Point tail) =>
    (
            (tail == head) ||

            (tail.X == head.X + 1 && tail.Y == head.Y - 1) ||
            (tail.X == head.X + 1 && tail.Y == head.Y) ||
            (tail.X == head.X + 1 && tail.Y == head.Y + 1) ||

            (tail.X == head.X && tail.Y == head.Y - 1) ||
            (tail.X == head.X && tail.Y == head.Y + 1) ||

            (tail.X == head.X - 1 && tail.Y == head.Y - 1) ||
            (tail.X == head.X - 1 && tail.Y == head.Y) ||
            (tail.X == head.X - 1 && tail.Y == head.Y + 1)
    );
}
