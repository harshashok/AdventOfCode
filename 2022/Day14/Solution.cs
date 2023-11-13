using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]      
class Solution : Solver {
    List<Point[]> paths = new();
    Dictionary<Point, Char> rocks = new();
    int edgeX, edgeY;

    public object PartOne(string input) {
        ReadInput(input);
        return FillSandSteps();
    }

    public object PartTwo(string input) {
        return 0;
    }

    private int FillSandSteps()
    {
        //sand source.
        var src = new Point(500, 0);
        int sandCount = 0;
        bool abyss = false;

        while (!abyss)
        {
            (Point sand, bool rest) sandPos = (src, false);
            while (sandPos.rest != true)
            {
                sandPos = Move(sandPos.sand);
                if (sandPos.rest == true)
                {
                    rocks.Add(sandPos.sand, 'o');
                    sandCount++;
                }
                if (sandPos.sand.Y >= edgeY)
                {
                    abyss = true;
                    break;
                }
            }
        }
        return sandCount;
    }

    private (Point,bool) Move(Point p)
    {
        var down = new Size(0, 1);
        var dleft = new Size(-1, 1);
        var dright = new Size(1, 1);

        if (!rocks.ContainsKey(p + down)) return ((p + down), false);
        if (!rocks.ContainsKey(p + dleft)) return ((p + dleft), false);
        if (!rocks.ContainsKey(p + dright)) return ((p + dright), false);
        return (p, true);

    }
    private void MapRocks(Point[] points)
    {
        for (int i = 1; i < points.Length; i++)
        {
            AddRocks(points[i - 1], points[i]);
        }
    }

    private void AddRocks(Point start, Point end)
    {
        Size diff = new Size(Math.Sign(end.X - start.X), Math.Sign(end.Y - start.Y));
        for (var item = start; item != end+diff; item= Point.Add(item,diff))
        {
            rocks[item] = '#';
        }
    }

    private void ReadInput(string input)
    {
        paths = input.Split('\n')
            .Select(x => x.Split(" -> "))
            .Select(l =>
                l.Select(p =>
                {
                    var arr = p.Split(',');
                    return new Point(int.Parse(arr[0]), int.Parse(arr[1]));
                })
                .ToArray())
            .ToList();
        paths.ForEach(x => MapRocks(x));

        edgeX = rocks.Select(x => x.Key).Select(p => p.X).Max();
        edgeY = rocks.Select(x => x.Key).Select(p => p.Y).Max();
    }
}
