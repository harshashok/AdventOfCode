using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]      
class Solution : Solver {
    Dictionary<Point, string> Map = new();
    List<Number> numMap = new();
    record Number (string value, Point point);
    Regex number_regex = new(@"\d+");
    Regex symbol_regex = new(@"[^.\d]");

    public object PartOne(string input) {
        ReadInput(input);
        return numMap.Select(num => IsPartNumber(num) ? int.Parse(num.value) : 0).Sum();
    }

    public object PartTwo(string input)
    {
        var gears = Map.Where(x => x.Value == "*");
        var gRatio = gears.Select(x => GearRatio(x.Key)).ToList();

        return (
            from g in gears
            let neighbours = from n in numMap where AltGearRatio(n, g.Key) select int.Parse(n.value)
            where neighbours.Count() == 2
            select neighbours.First() * neighbours.Last()
        ).Sum();
    }

    /**
     *
     *     bool Adjacent(Part p1, Part p2) =>
            Math.Abs(gear.X - p1.X) <= 1 &&
            p1.Y <= gear.Y + gear.Text.Length &&
            gear.Y <= p1.Y + p1.Text.Length;
     */

    /*
     *  .....
     *  .234.
     *  ....*
     */
    bool AltGearRatio(Number num, Point gear)
    {
        return Math.Abs(gear.X - num.point.X) <= 1 &&
            num.point.Y <= gear.Y + 1 &&
            gear.Y <= num.point.Y + num.value.Length;
    }
    
    int GearRatio(Point point)
    {
        var local = GridOffset.Select(size =>
            {
                //return IsPartNumber(point+size) ? int.Parse(Map[point + size]) : -1;
                return Map.TryGetValue(point + size, out string val) && (int.TryParse(val, out int intValue))
                    ? intValue
                    : -1;
            })
            .Where(x => x >= 0)
            .ToList();
        
        var result = 0;
        if (local.Count == 2)
        {
            result = local.First() * local.Last();
        } 
        
        return result;
    }

    private List<Size> GridOffset = new()
    {
        new Size(-1, -1),
        new Size(-1, 0),
        new Size(-1, 1),
        new Size(0, -1),
        new Size(0, 1),
        new Size(1, -1),
        new Size(1, 0),
        new Size(1, 1)
    };

    /**
     *    .....
     *    .234.
     *    .....
     */
    bool IsPartNumber(Point point)
    {
        return !IsSymbol(point) && (Map.TryGetValue(point, out string value)) && IsPartNumber(new Number(value, point));
    }
    bool IsPartNumber(Number part)
    {
        int numLength = part.value.Length;
        Point start = part.point - new Size(0,1);
        Point end = part.point + new Size(0, numLength);

        if (IsSymbol(start) || IsSymbol(end)) return true;

        //check top rect line
        for (var top = start + new Size(-1,0); top != end + new Size(-1,1); top += new Size(0, 1))
        {
            if (IsSymbol(top)) return true;
        }

        //check bottom rect line
        for (var bottom = start + new Size(1, 0); bottom != end + new Size(1, 1); bottom += new Size(0, 1))
        {
            if (IsSymbol(bottom)) return true;
        }  
        return false;
    }

    bool IsSymbol(Point p) => Map.TryGetValue(p, out string val) && (!int.TryParse(val, out _));

    void ReadInput(string input)
    {
        int index = 0;
        input.Split('\n').ToList()
            .ForEach((line => ParseLine(line, index++)));
    }

    void ParseLine(string line, int lineNumber)
    {
        var number_result = number_regex.Matches(line);
        var symbol_result = symbol_regex.Matches(line);

        number_result.ToList()
            .ForEach(m =>
            {
                Point startpoint = new Point(lineNumber, m.Index);
                Point endPoint = startpoint + new Size(0, m.Length);
                numMap.Add(new Number(m.Value, startpoint));
                for (var start = startpoint; start != endPoint; start+=new Size(0, 1))
                {
                    Map.Add(start, m.Value);
                }
            });

        symbol_result.ToList().ForEach(m => Map.Add(new Point(lineNumber, m.Index), m.Value));
    }
}
