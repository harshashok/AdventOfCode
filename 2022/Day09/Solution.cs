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
        int movectr = 0;
        foreach (var move in moves)
        {
            SimulateSingleMoveSteps(move);
            movectr++;
        }
    }

    private void SimulateSingleMoveSteps(Move move)
    {
        for (int i = 0; i < move.steps; i++)
        {
            moveInDirection(move.dir, ref knots[0]);
            for (int k = 0; k < knots.Length - 1; k++)
            {

                if (!InVicinity(knots[k], knots[k + 1]))
                {
                    CalculateOffSet(ref knots[k], ref knots[k +1]);
                    visited.Add(new Point(knots[^1].X, knots[^1].Y));
                }
            }
        }
    }

    private void moveInDirection(string dir, ref Point node)
    {
        switch (dir)
        {
            case "U":
                node.Offset(1, 0);
                break;
            case "D":
                node.Offset(-1, 0);
                break;
            case "L":
                node.Offset(0, -1);
                break;
            case "R":
                node.Offset(0, 1);
                break;
            default:
                break;
        }
    }

    /* Offset calc
    *   (x+2,y-2)  (x+2,y-1)         (x+2,y+1)  (x+2,y+2)
    *   (x+1,y-2) | x+1,y-1 | x+1,y | x+1,y+1 | (x+1,y+2)
    *             |  x,y-1  | x,y   |  x,y+1  |
    *   (x-1,y-2) | x-1,y-1 | x-1,y | x-1,y+1 | (x-1,y+2)   
    *   (x-2,y-2)  (x-2,y-1)         (x-2,y+1)  (x-2,y+2)
    */
    private void CalculateOffSet(ref Point head, ref Point tail)
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
            if (tail.X == head.X + 2 && (tail.Y == head.Y - 1 || tail.Y == head.Y + 1)) tail = new Point(head.X + 1, head.Y);
            if (tail.X == head.X - 2 && (tail.Y == head.Y - 1 || tail.Y == head.Y + 1)) tail = new Point(head.X - 1, head.Y);
            if (tail.Y == head.Y + 2 && (tail.X == head.X + 1 || tail.X == head.X - 1)) tail = new Point(head.X, head.Y + 1);
            if (tail.Y == head.Y - 2 && (tail.X == head.X + 1 || tail.X == head.X - 1)) tail = new Point(head.X, head.Y - 1);

            if (tail.Y == head.Y - 2 && tail.X == head.X + 2) tail = new Point(head.X + 1, head.Y - 1);
            if (tail.Y == head.Y - 2 && tail.X == head.X - 2) tail = new Point(head.X - 1, head.Y - 1);

            if (tail.Y == head.Y + 2 && tail.X == head.X - 2) tail = new Point(head.X - 1, head.Y + 1);
            if (tail.Y == head.Y + 2 && tail.X == head.X + 2) tail = new Point(head.X + 1, head.Y + 1);
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
        visited.Add(tail);
    }
}
