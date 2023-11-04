using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;

namespace AdventOfCode.Y2022.Day12;

[ProblemName("Hill Climbing Algorithm")]      
class Solution : Solver {

    private char[][] gridMap;
    int X, Y;  //grid length, height.
    Dictionary<Point, Point?> cameFrom = new(); //path A->B is stored as came_from[B] == A | key:B val:A
    Dictionary<Point, int> costSoFar = new();
    private Point start, end;
    private List<Point> allGoals = new();

    public object PartOne(string input) {
        ReadInput(input);
        AStarSearch();
        return GetPathDistanceFromPoint(start);
    }

    public object PartTwo(string input) {
        return allGoals.Select(x => GetPathDistanceFromPoint(x)).Min();
    }

    //Searching from the end goal 
    private void AStarSearch()
    {
        PriorityQueue<Point, int> frontier = new();
        List<Point> visited = new();
        frontier.Enqueue(end, 0);
        cameFrom[end] = null;
        costSoFar[end] = 0;
        
        while (frontier.TryPeek(out _, out _))
        {
            var curr = frontier.Dequeue();
            visited.Add(curr);

            foreach (var next in GetEligibleNeigbors(curr))
            {
                if (visited.Contains(next)) continue;

                int newCost = costSoFar[curr] + 1;
                if (!costSoFar.TryGetValue(next, out _) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(end, next);
                    frontier.Enqueue(next, (int)priority);
                    cameFrom[next] = curr;
                }
            }
        }
    }

    private int GetPathDistanceFromPoint(Point starting)
    {
        List<Point> path = new();
        Point current = starting;
        while (current != end)
        {
            path.Add(current);
            if (!cameFrom.TryGetValue(current, out _)) return Int32.MaxValue;
            current = (Point)cameFrom[current];
        }
        path.Append(end);

        return path.Count;
    }

    /**
     *             (x-1,y)
     *    (x,y-1) | (x,y) | (x,y+1)
     *             (x+1,y)
    **/
    private List<Point> GetEligibleNeigbors(Point point)
    {
        List<Point> validNbors = new();
        if (EligibleNbor(point, point.X-1, point.Y)) validNbors.Add(new Point(point.X-1, point.Y));
        if (EligibleNbor(point, point.X, point.Y-1)) validNbors.Add(new Point(point.X, point.Y-1));
        if (EligibleNbor(point, point.X, point.Y+1)) validNbors.Add(new Point(point.X, point.Y+1));
        if (EligibleNbor(point, point.X+1, point.Y)) validNbors.Add(new Point(point.X+1, point.Y));
        return validNbors;
    }

    private bool EligibleNbor(Point curr, int x, int y)
    {
        var isValid = ValidGridPoint(x, y);
        var retVal = isValid && (gridMap[curr.X][curr.Y] - gridMap[x][y]) <= 1;
        return retVal;
    }

    private bool ValidGridPoint(int x, int y) => (x >= 0) && (x < X) && (y >= 0) && (y < Y);

    private double Heuristic(Point goal, Point next) => Math.Abs(goal.X - next.X) + Math.Abs(goal.Y - next.Y);

    private void ReadInput(string input)
    {
        gridMap = input.Split('\n')
            .Select((x, line) =>
            {
                var startEnd = x.Select((c, index) => (c, index))
                    .Where(pnt => pnt.c == 'S' || pnt.c == 'E' || pnt.c == 'a');
                InitStartEndPoints(startEnd, line);
                return x.ToArray();
            })
            .ToArray();
        SetStartEndPoints(start, end);
        X = gridMap.Length;
        Y = gridMap[0].Length;
    }

    private void InitStartEndPoints(IEnumerable<(char c, int i)> startEnd, int line)
    {
        if (!startEnd.Any()) return;
        foreach(var x in startEnd)
        {
            if (x.c == 'S') start = new Point(line, x.i);
            if (x.c == 'E') end = new Point(line, x.i);
            if (x.c == 'a') allGoals.Add(new Point(line, x.i));
        }
    }

    private void SetStartEndPoints(Point start, Point end)
    {
        gridMap[start.X][start.Y] = 'a';
        gridMap[end.X][end.Y] = 'z';
        allGoals.Insert(0, start);
    }
}


/** 
 * Things to do
 * 1. Parse to grid
 * 2. Find starting point (S) and ending point (E) - do this while parsing?
 * 3. method func to eval next step with S,E taken into account.
 * 4. A* Algorithm
 * 
**/
