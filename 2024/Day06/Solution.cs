using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;


namespace AdventOfCode.Y2024.Day06;

[ProblemName("Guard Gallivant")]      
class Solution : Solver
{

    private ImmutableDictionary<Point, char> gridMap;
    private Point gPosition;

    public object PartOne(string input)
    {
        ReadInput(input);
        return Patrol(gridMap, gPosition).Count;
    }

    public object PartTwo(string input) =>
        Patrol(gridMap, gPosition)
            .AsParallel()
            .Count(pos => PatrolLoop(gridMap.SetItem(pos, '#'), gPosition));

    private bool PatrolLoop(ImmutableDictionary<Point, char> map, Point currPosition)
    {
        var dir = '^';
        HashSet<(Point, char)> visited = new();
        
        while (map.ContainsKey(currPosition) && !visited.Contains((currPosition, dir)))
        {
            visited.Add((currPosition, dir));
            Point nextP = Move(currPosition, dir);
            
            if (map.GetValueOrDefault(nextP) == '#')
            {
                dir = TurnRight(dir);
            }
            else
            {
                currPosition = nextP;
            }
        }
        return visited.Contains((currPosition, dir));
    }

    private HashSet<Point> Patrol(ImmutableDictionary<Point, char> map, Point currPosition)
    {
        var dir = '^';
        HashSet<Point> visited = new();
        
        while (map.ContainsKey(currPosition))
        {
            visited.Add(currPosition);
            Point nextP = Move(currPosition, dir);
            if (!map.ContainsKey(nextP)) break;
            
            if (map[nextP] == '.')
            {
                currPosition = nextP;
            }
            else
            {
                dir = TurnRight(dir);
            }
        }
        return visited;
    }

    private Point Move(Point node, char ch) =>
        ch switch
        {
            '^' => new Point(node.X - 1, node.Y),
            'v' => new Point(node.X + 1, node.Y),
            '<' => new Point(node.X, node.Y - 1),
            '>' => new Point(node.X, node.Y + 1),
            _ => throw new ArgumentOutOfRangeException(ch.ToString())
        };

    private char TurnRight(char ch) =>
        ch switch
        {
            '^' => '>',
            'v' => '<',
            '<' => '^',
            '>' => 'v',
            _ => throw new ArgumentOutOfRangeException(ch.ToString())
        };

    private void ReadInput(string input)
    {
        var lines = input.Split('\n');

        gridMap = (from x in Enumerable.Range(0, lines.Length)
                   from y in Enumerable.Range(0, lines[0].Length)
                   select new KeyValuePair<Point, char>(new Point(x, y), lines[x][y])
                  ).ToImmutableDictionary(pair => pair.Key, pair => pair.Value);

        gPosition = gridMap.First(p => p.Value == '^').Key;
        gridMap.SetItem(gPosition, '.');
    }
}
