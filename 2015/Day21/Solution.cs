using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day21;

[ProblemName("RPG Simulator 20XX")]
class Solution : Solver
{
    private IEnumerable<Item> _weaponStats;
    private IEnumerable<Item> _armorStats;
    private IEnumerable<Item> _ringStats;
    private IEnumerable<IEnumerable<Item>> ringCombinations;
    private CharStats Player;
    private CharStats Boss;
    private HashSet<(bool game, int cost)> gameCost = new();

    record Item(string name, int cost, int damage, int armor);
    record CharStats(string name, int HP, int damage, int armor);

    public object PartOne(string input)
    {
        ReadInput(input);
        GenerateGameSets();
        return gameCost.Where(t => t.game).Min(t => t.cost);
    }

    public object PartTwo(string input) => gameCost.Where(t => t.game == false).Max(t => t.cost);

    private void GenerateGameSets()
    {
        foreach (var weapon in _weaponStats)
            foreach (var armor in _armorStats)
                foreach (var ringCombo in ringCombinations)
                {
                    Player = new CharStats("player", 
                        HP: 100,
                        damage: weapon.damage + armor.damage + ringCombo.Sum(x => x.damage),
                        armor: weapon.armor + armor.armor + ringCombo.Sum(x => x.armor));

                    gameCost.Add((PlayGame(Player, Boss), weapon.cost + armor.cost + ringCombo.Sum(x => x.cost)));
                }
    }

private bool PlayGame(CharStats player, CharStats boss)
    {
        for (int i = 0;; i++)
        {
            if (i % 2 == 0)      //Player's Turn
            {
                int effectiveDamage = player.damage - boss.armor;
                boss = boss with { HP = boss.HP - effectiveDamage };
                if (boss.HP <= 0) return true;
            }
            else        //Boss' Turn
            {
                int effectiveDamage = Boss.damage - Player.armor;
                player = player with { HP = player.HP - effectiveDamage };
                if (player.HP <= 0) return false;
            } 
        }
    }

    private void ReadInput(string input)
    {
        var blocks = input.Split("\n\n");

        _weaponStats = ParseStats(blocks[0]);
        _armorStats = ParseStats(blocks[1]);
        _ringStats = ParseStats(blocks[2]).ToList();
        
        _armorStats = _armorStats.Append(new Item("NoArmor", 0, 0, 0));

        var combo0Rings = Enumerable.Repeat(Enumerable.Repeat(new Item("NoRings", 0, 0, 0), 1), 1).ToList();
        var combo1Rings = _ringStats.GetCombinations(1).ToList();
        var combo2Rings = _ringStats.GetCombinations(2).ToList();
        ringCombinations = combo0Rings.Concat(combo1Rings).Concat(combo2Rings).ToList();

        var bossStats = blocks[3].Split('\n')
            .Select(line => line.Split(':')[1].Trim())
            .Select(Int32.Parse).ToArray();

        Boss = new CharStats("boss", bossStats[0], bossStats[1], bossStats[2]);
    }
    
    private IEnumerable<Item> ParseStats(string block) =>
        block.Split('\n').Skip(1)
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(w => new Item(name: w[0], cost: Int32.Parse(w[1]), damage: Int32.Parse(w[2]), armor: Int32.Parse(w[3])));
}
