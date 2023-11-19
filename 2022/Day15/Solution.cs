using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;

namespace AdventOfCode.Y2022.Day15;

[ProblemName("Beacon Exclusion Zone")]      
class Solution : Solver {
    Dictionary<Point, Point> sensorBeacon = new();
    HashSet<Point> fields = new();

    public object PartOne(string input)
    {
        ReadInput(input);
        sensorBeacon.ToList().ForEach(pair => FillSensorField(pair.Key, pair.Value, 2000000));
        //var xx = fields.Where(v => v.Y == 10);
        return fields.Count();
    }

    public object PartTwo(string input) {
        return 0;
    }

    void FillSensorField(Point sensor, Point beacon, int? row = null)
    {
        int dist = ManhattanDistance(sensor, beacon);

        var leftStart = sensor - new Size(dist,0);
        var rightStart = sensor + new Size(dist,0);

        while(leftStart != rightStart)
        {
            FillLine(leftStart, rightStart, row);
            leftStart += new Size(1, -1);
            rightStart -= new Size(1, 1);
        }
        AddFieldPoint(leftStart, row);

        leftStart = sensor - new Size(dist, 0);
        rightStart = sensor + new Size(dist, 0);

        while (leftStart != rightStart)
        {
            FillLine(leftStart, rightStart, row);
            leftStart += new Size(1, 1);
            rightStart -= new Size(1, -1);
        }
        AddFieldPoint(leftStart, row);

    }

    void FillLine(Point start, Point end, int? row = null)
    {
        if (row != null && start.Y != row) return;

        Size diff = new Size(Math.Sign(end.X - start.X), Math.Sign(end.Y - start.Y));
        for (var item = start; item != end + diff; item = item + diff)
        {
            AddFieldPoint(item, row);
        }
    }

    void AddFieldPoint(Point point, int? row = null)
    {
        if (row != null && point.Y != row) return;
        if (!sensorBeacon.ContainsKey(point) && !sensorBeacon.ContainsValue(point))
        {
            fields.Add(point);
        }
    }

    void ReadInput(string input)
    {
        Regex regex = new(@"-?\d+");
        input.Split('\n')
            .Select(line => regex.Matches(line))
            .Select(m => m.Select(m => m.Value))
            .Select(p => Array.ConvertAll(p.ToArray(), int.Parse))
            .ToList()
            .ForEach(p => sensorBeacon.Add(new Point(p[0], p[1]), new Point(p[2], p[3])));
    }

    static int ManhattanDistance(Point start, Point end) => Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
}
