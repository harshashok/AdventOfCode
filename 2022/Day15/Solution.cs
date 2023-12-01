using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Collections;

namespace AdventOfCode.Y2022.Day15;

[ProblemName("Beacon Exclusion Zone")]      
class Solution : Solver {
    Dictionary<Point, Point> sensorBeacon = new();
    HashSet<Point> fields;
    int bounds;

    public object PartOne(string input)
    {
        ReadInput(input);
        int row = 2000000;
        sensorBeacon.ToList().ForEach(pair => FillSensorField(pair.Key, pair.Value, row));
        return fields.Count;
    }

    public object PartTwo(string input) {
        bounds = 4000000;
        return FindDistressBeacon();
    }

    /**
     * PART 2 : 
     * for every sensor
        go around its diamond path, just outside it (+/-1) and for every point
        check every other sensor and count how many have a Manhattan distance currentPoint->otherSensor equal to their otherSensor->otherBeacon distance+1
            if I find a point that is just outside 3 other diamonds then it's the solution
    **/
    long FindDistressBeacon()
    {
        Dictionary<Point, int> packCount = new();
        foreach (var currItem in sensorBeacon)
        {
            var sensor = currItem.Key;
            var beacon = currItem.Value;
            int distance = ManhattanDistance(sensor, beacon);

            Point top = new Point(sensor.X, sensor.Y - distance - 1);    //+1 top of diamond.
            Point bottom = new Point(sensor.X, sensor.Y + distance + 1); //+1 bottom of diamond.
            Point left = new Point(sensor.X - distance - 1, sensor.Y);   //+1 left of diamond.
            Point right = new Point(sensor.X + distance + 1, sensor.Y);  //+1 right of diamond.

            //from top -> right
            Size diff = new Size(Math.Sign(right.X - top.X), Math.Sign(right.Y - top.Y));
            for (var currPoint = top; currPoint != right; currPoint = currPoint + diff)
            {
                if (IsPointDistressBeacon(currPoint, currItem))
                {
                    return currPoint.X * 4000000L + currPoint.Y;
                }
            }

            //from right -> bottom
            diff = new Size(Math.Sign(bottom.X - right.X), Math.Sign(bottom.Y - right.Y));
            for (var currPoint = right; currPoint != bottom; currPoint = currPoint + diff)
            {
                if (IsPointDistressBeacon(currPoint, currItem))
                {
                    return currPoint.X * 4000000L + currPoint.Y;
                }
            }

            //from bottom -> left
            diff = new Size(Math.Sign(left.X - bottom.X), Math.Sign(left.Y - bottom.Y));
            for (var currPoint = bottom; currPoint != left; currPoint = currPoint + diff)
            {
                if (IsPointDistressBeacon(currPoint, currItem))
                {
                    return currPoint.X * 4000000L + currPoint.Y;
                }
            }

            //from left -> top
            diff = new Size(Math.Sign(top.X - left.X), Math.Sign(top.Y - left.Y));
            for (var currPoint = left; currPoint != top; currPoint = currPoint + diff)
            {
                if (IsPointDistressBeacon(currPoint, currItem))
                {
                    return currPoint.X * 4000000L + currPoint.Y;
                }
            }
        }

        return -1;
    }

    bool IsPointDistressBeacon(Point currPoint, KeyValuePair<Point, Point> currSensor)
    {
        if (!InBounds(currPoint)) return false;

        int count = 0;
        foreach (var other in sensorBeacon)
        {
            if (other.Key == currSensor.Key) continue;

            var pointOtherSensorDist = ManhattanDistance(currPoint, other.Key);
            var otherSensorBeaconDist = ManhattanDistance(other.Key, other.Value);

            if (pointOtherSensorDist <= otherSensorBeaconDist) return false;
            if (pointOtherSensorDist == otherSensorBeaconDist + 1) count++;
        }
        return count > 2;
    }

    bool InBounds(Point point) => (point.X >= 0 && point.X <= bounds) && (point.Y >= 0 && point.Y <= bounds);

    static int ManhattanDistance(Point start, Point end) => Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);

    /**
     * PART 1 soln methods.
     */
    void FillSensorField(Point sensor, Point beacon, int? row = null)
    {
        int dist = ManhattanDistance(sensor, beacon);

        var leftStartUp = sensor - new Size(dist,0);
        var rightStartUp = sensor + new Size(dist,0);

        var leftStartDown = sensor - new Size(dist, 0);
        var rightStartDown = sensor + new Size(dist, 0);

        while (leftStartUp != rightStartUp || leftStartDown != rightStartDown)
        {
            FillLine(leftStartUp, rightStartUp, row);
            leftStartUp += new Size(1, -1);
            rightStartUp -= new Size(1, 1);

            FillLine(leftStartDown, rightStartDown, row);
            leftStartDown += new Size(1, 1);
            rightStartDown -= new Size(1, -1);
        }
        AddFieldPoint(leftStartUp, row);
        AddFieldPoint(leftStartDown, row);
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
        if (!sensorBeacon.ContainsKey(point) && !sensorBeacon.ContainsValue(point)) fields.Add(point);
    }

    void ReadInput(string input)
    {
        fields = new();
        Regex regex = new(@"-?\d+");
        input.Split('\n')
            .Select(line => regex.Matches(line))
            .Select(m => m.Select(m => m.Value))
            .Select(p => Array.ConvertAll(p.ToArray(), int.Parse))
            .ToList()
            .ForEach(p => sensorBeacon.Add(new Point(p[0], p[1]), new Point(p[2], p[3])));
    }
}
