using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Y2015.Day03;

[ProblemName("Perfectly Spherical Houses in a Vacuum")]      
class Solution : Solver
{
    private HashSet<Point> visited = new();
    public object PartOne(string input)
    {
        Point current = new Point(0, 0);
        visited.Add(current);
        input.ToList()
            .ForEach(ch =>
            {
                current = Move(current, ch);
                visited.Add(current); 
            });
        return visited.Count;
    }

    public object PartTwo(string input)
    {
        visited = new();
        Point santa = new Point(0, 0);
        Point roboSanta = new Point(0, 0);
        visited.Add(santa);
        
        input.Chunk(2).ToList()
             .ForEach(x =>
                 {
                     santa = Move(santa, x[0]);
                     roboSanta = Move(roboSanta, x[1]);
                     visited.Add(santa);
                     visited.Add(roboSanta);
                 }
             );
        
        return visited.Count;
    }
    
    Point Move(Point node, char ch) =>
        ch switch
        {
            '^' => new Point(node.X + 1, node.Y),
            'v' => new Point(node.X - 1, node.Y),
            '<' => new Point(node.X, node.Y - 1),
            '>' => new Point(node.X, node.Y + 1),
            _ => throw new ArgumentOutOfRangeException(ch.ToString())
        };
}
