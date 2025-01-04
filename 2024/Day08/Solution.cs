using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.Y2024.Day08;

[ProblemName("Resonant Collinearity")]      
class Solution : Solver
{
    private ImmutableDictionary<Point, char> gridMap;
    private Dictionary<Point, char> antennas;

    public object PartOne(string input) {
        ReadInput(input);
        return SolveLinq().Distinct().Count();
    }

    public object PartTwo(string input) => Solve2Linq().Count;

    private IEnumerable<Point> SolveLinq() =>
        antennas.Select(antenna => 
                    antennas.Where(a => a.Value == antenna.Value && a.Key != antenna.Key)
                    .Select(other =>
                    {
                        var delta = Delta(antenna.Key, other.Key);
                        return new Point(antenna.Key.X + delta.row, antenna.Key.Y + delta.col);
                    })
                    .Where(point => gridMap.ContainsKey(point)))
                .SelectMany(a => a);

    private HashSet<Point> Solve2Linq()
    {
        HashSet<Point> antinodes = new();
        antennas.ToList().ForEach(antenna =>
            antennas.Where(a => a.Value == antenna.Value && a.Key != antenna.Key).ToList()
                .ForEach(other =>
                {
                    var delta = Delta(antenna.Key, other.Key);
                    var potentialAntiNode = new Point(antenna.Key.X + delta.row, antenna.Key.Y + delta.col);

                    while (gridMap.ContainsKey(potentialAntiNode))
                    {
                        antinodes.Add(potentialAntiNode);
                        potentialAntiNode = new Point(potentialAntiNode.X + delta.row, potentialAntiNode.Y + delta.col);
                    }

                    potentialAntiNode = new Point(antenna.Key.X - delta.row, antenna.Key.Y - delta.col);
                    while (gridMap.ContainsKey(potentialAntiNode))
                    {
                        antinodes.Add(potentialAntiNode);
                        potentialAntiNode = new Point(potentialAntiNode.X - delta.row, potentialAntiNode.Y - delta.col);
                    }
                }));
        return antinodes;
    }
    
    private HashSet<Point> Solve()
    {
        HashSet<Point> antinodes = new();
        foreach (var antenna in antennas)
        {
            var sameFrequencyAntennas = antennas.Where(a => a.Value == antenna.Value && a.Key != antenna.Key);

            foreach (var other in sameFrequencyAntennas)
            {
                var delta = Delta(antenna.Key, other.Key);
                var potentialAntiNode = new Point(antenna.Key.X + delta.row, antenna.Key.Y + delta.col);
                if (gridMap.ContainsKey(potentialAntiNode))
                {
                    antinodes.Add(potentialAntiNode);
                }
            }
        }
        return antinodes;
    }
    
    private HashSet<Point> Solve2()
    {
        HashSet<Point> antinodes = new();
        foreach (var antenna in antennas)
        {
            var sameFrequencyAntennas = antennas.Where(a => a.Value == antenna.Value && a.Key != antenna.Key);

            foreach (var other in sameFrequencyAntennas)
            {
                var delta = Delta(antenna.Key, other.Key);
                var potentialAntiNode = new Point(antenna.Key.X + delta.row, antenna.Key.Y + delta.col);
                while(gridMap.ContainsKey(potentialAntiNode))
                {
                    antinodes.Add(potentialAntiNode);
                    potentialAntiNode = new Point(potentialAntiNode.X + delta.row, potentialAntiNode.Y + delta.col);
                }

                potentialAntiNode = new Point(antenna.Key.X - delta.row, antenna.Key.Y - delta.col);
                while(gridMap.ContainsKey(potentialAntiNode))
                {
                    antinodes.Add(potentialAntiNode);
                    potentialAntiNode = new Point(potentialAntiNode.X - delta.row, potentialAntiNode.Y - delta.col);
                }
            }
        }
        return antinodes;
    }
    
    private (int row, int col) Delta(Point p1, Point p2) => (row: p1.X - p2.X, col: p1.Y - p2.Y); 
    
    private void ReadInput(string input)
    {
        var lines = input.Split('\n');

        gridMap = (from x in Enumerable.Range(0, lines.Length)
                from y in Enumerable.Range(0, lines[0].Length)
                select new KeyValuePair<Point, char>(new Point(x, y), lines[x][y])
            ).ToImmutableDictionary(pair => pair.Key, pair => pair.Value);

        antennas = gridMap.Where(a => a.Value != '.')
            .ToDictionary(pair => pair.Key, pair => pair.Value );
    }
}
