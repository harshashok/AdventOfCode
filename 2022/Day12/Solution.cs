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
    private Point start, end;

    public object PartOne(string input) {
        ReadInput(input);
        return AStarSearch();
    }

    public object PartTwo(string input) {
        return 0;
    }

    private void Dijkstra()
    {

    }
    private int AStarSearch()
    {
        PriorityQueue<Point, int> frontier = new();
        List<Point> visited = new();
        Dictionary<Point, Point?> cameFrom = new(); //path A->B is stored as came_from[B] == A | key:B val:A
        Dictionary<Point, int> costSoFar = new();
        frontier.Enqueue(start, 0);
        cameFrom[start] = null;
        costSoFar[start] = 0;

        while (frontier.Peek() != null)
        {
            var curr = frontier.Dequeue();
            visited.Add(curr);
            if (curr == end) break;

            foreach (var next in GetEligibleNeigbors(curr))
            {
                if (visited.Contains(next))
                {
                    continue;
                }

                int newCost = costSoFar[curr] + 1; //TODO : this needs to be a real cost system
                if (!costSoFar.TryGetValue(next, out _) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    var priority = newCost + Heuristic(end, next);
                    frontier.Enqueue(next, (int)priority);
                    cameFrom[next] = curr;
                }
            }
        }

        List<Point> path = new();
        Point current = end;
        while(current != start)
        {
            path.Add(current);
            current = (Point)cameFrom[current];
        }
        path.Append(start);

        return path.Count;
    }

    private int CostCalculator(Point curr, Point next)
    {
        var value = (gridMap[next.X][next.Y] - gridMap[curr.X][curr.Y]);

        if (value == 0 || value == 1) return 1;
        if (value < 0) return Math.Abs(value);
        return value;
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

    private bool EligibleNbor_old(Point curr, int x, int y)
    {
        var isValid = ValidGridPoint(x, y);
        var retVal = isValid && (gridMap[curr.X][curr.Y] == gridMap[x][y] || gridMap[curr.X][curr.Y] + 1 == gridMap[x][y]);
        return retVal;
    }

    private bool EligibleNbor(Point curr, int x, int y)
    {
        var isValid = ValidGridPoint(x, y);
        var retVal = isValid && (gridMap[x][y] - gridMap[curr.X][curr.Y]) <= 1;
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
                    .Where(pnt => pnt.c == 'S' || pnt.c == 'E');
                InitStartEndPoints(startEnd, line);
                return x.ToArray();
            })
            .ToArray();
        SetStartEndPoints();
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
        }
    }

    private void SetStartEndPoints()
    {
        gridMap[start.X][start.Y] = 'a';
        gridMap[end.X][end.Y] = 'z';
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