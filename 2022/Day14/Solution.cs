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
    Dictionary<Point, Char> rocks;
    int edgeY;
    bool floor;

    public object PartOne(string input) {
        floor = false;
        ReadInput(input);
        return FillSandSteps();
    }

    public object PartTwo(string input) {
        floor = true;
        return FillSandSteps();
    }

    private int FillSandSteps()
    {
        //sand source.
        var src = new Point(500, 0);
        bool abyssFloor = false;
        while (!abyssFloor)
        {
            (Point sand, bool rest) sandPos = (src, false);
            while (sandPos.rest != true)
            {
                sandPos = Move(sandPos.sand);
                if (rocks.ContainsKey(sandPos.sand)) { abyssFloor = true; break; }
                if (sandPos.rest == true) rocks.Add(sandPos.sand, 'o');
                if (!floor && sandPos.sand.Y > edgeY) { abyssFloor = true; break; }
            }
        }
        return rocks.Select(y => y.Value).Count(v => v == 'o');
    }

    private (Point,bool) Move(Point p)
    {
        var down = new Size(0, 1);
        var dleft = new Size(-1, 1);
        var dright = new Size(1, 1);

        /*part2*/
        if (floor && (p.Y == edgeY + 1)) return (p, true);

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
        for (var item = start; item != end+diff; item= item+diff)
        {
            rocks[item] = '#';
        }
    }

    private void ReadInput(string input)
    {
        rocks = new();
        input.Split('\n')
            .Select(x => x.Split(" -> "))
            .Select(l =>
                l.Select(p =>
                {
                    var arr = p.Split(',');
                    return new Point(int.Parse(arr[0]), int.Parse(arr[1]));
                })
                .ToArray())
            .ToList()
            .ForEach(x => MapRocks(x));

        edgeY = rocks.Select(x => x.Key).Select(p => p.Y).Max();
    }
}
