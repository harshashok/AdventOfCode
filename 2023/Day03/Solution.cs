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
    Regex numberRegex = new(@"\d+");
    Regex symbolRegex = new(@"[^.\d]");

    public object PartOne(string input) {
        ReadInput(input);
        return numMap.Where(IsPartNumber).Sum(num => int.Parse(num.value));
    }

    public object PartTwo(string input)
    {
        var gears = Map.Where(x => x.Value == "*");
        return (
            from g in gears
            let neighbours = from num in numMap where GearRatio(num, g.Key) select int.Parse(num.value)
            where neighbours.Count() == 2
            select neighbours.First() * neighbours.Last()
        ).Sum();
    }
    
    /*
     *  .....
     *  .234.
     *  ....*
     */
    bool GearRatio(Number num, Point gear)
    {
        return Math.Abs(gear.X - num.point.X) <= 1 &&
            num.point.Y <= gear.Y + 1 &&
            gear.Y <= num.point.Y + num.value.Length;
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
        numberRegex.Matches(line)
                .ToList()
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
        
        symbolRegex.Matches(line).ToList().ForEach(m => Map.Add(new Point(lineNumber, m.Index), m.Value));
    }
}
