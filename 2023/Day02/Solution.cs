using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static AdventOfCode.Y2023.Day02.Solution;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]      
class Solution : Solver {
    List<Game> games = new();

    public object PartOne(string input) {
        ReadInput(input);
        var cmp = new Cubes(12, 13, 14);
        return games.Where(game => game.sets.All(g => (g.red <= cmp.red) && (g.green <= cmp.green) && g.blue <= cmp.blue))
            .Select(game => game.ID)
            .Sum();
    }

    public object PartTwo(string input) {
        return games.Select(game =>
        {
            int red = 0, green = 0, blue = 0;
            game.sets.ForEach(g => {
                red = Math.Max(red, g.red);
                green = Math.Max(green, g.green);
                blue = Math.Max(blue, g.blue);
            });
            return red * green * blue;
        })
        .Sum();
    }

    void ReadInput(string input)
    {
        var aa = input.Split('\n');
        aa.ToList().ForEach(line => ParseGameSets(line));
    }

    void ParseGameSets(string line)
    {
        var idValues = line.Split(':');
        Game game = new();
        game.ID = int.Parse(Regex.Match(idValues[0], @"\d+").Value);

        idValues[1].Split(';')
            .Select(set => set.Split(','))
            .ToList()
            .ForEach(cubes =>
            {
                Dictionary<string, int> cubeMap = new();
                foreach (var cube in cubes)
                {
                    var value = cube.Trim().Split();
                    cubeMap.Add(value[1], int.Parse(value[0]));
                }
                game.sets.Add(new Cubes(
                        cubeMap.TryGetValue("red", out int red) ? red : 0,
                        cubeMap.TryGetValue("green", out int green) ? green : 0,
                        cubeMap.TryGetValue("blue", out int blue) ? blue : 0
                    ));
            });
        games.Add(game);
    }

    internal class Game
    {
        public int ID;
        public List<Cubes> sets = new();
    }

    internal record Cubes (int red, int green, int blue);
}
