using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day04;

[ProblemName("Scratchcards")]      
class Solution : Solver
{
    private List<Card> cards = new();
    public object PartOne(string input)
    {
        ReadInput(input);
        return cards.Select(card => card.winNumbers.Intersect(card.givenNumbers).Count())
            .Select(matches => matches > 0 ? Math.Pow(2, matches-1) : 0)
            .Sum();
    }

    public object PartTwo(string input)
    {
        var cardMatches = cards.Select(card => card.winNumbers.Intersect(card.givenNumbers).Count()).ToArray();
        int[] counts = new int[cardMatches.Length];
        Array.Fill(counts, 1);

        for (var i = 0; i < cardMatches.Length; i++)
        {
            for (var j = 0; j < cardMatches[i]; j++)
            {
                counts[i + j + 1] += counts[i];
            }
        }
        return counts.Sum();
    }

    void ParseCard(string line)
    {
        var blocks = line.Split(':', '|');
        Card card = new Card();
        card.cardNumber = int.Parse(Regex.Match(blocks[0], "[0-9]+").Value);
        var winNumbers = from m in Regex.Matches(blocks[1], @"\d+") select m.Value;
        var givenNumbers = from m in Regex.Matches(blocks[2], @"\d+") select m.Value;
        card.winNumbers = new(winNumbers);
        card.givenNumbers = new(givenNumbers);
        cards.Add(card);
    }
    
    void ReadInput(string input)
    {
        input.Split('\n').ToList()
            .ForEach(line => ParseCard(line));
    }

    class Card
    {
        public int cardNumber;
        public HashSet<string> winNumbers;
        public HashSet<string> givenNumbers;
    }
}
