using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            //            const string testInput = @"light red bags contain 1 bright white bag, 2 muted yellow bags.
            //dark orange bags contain 3 bright white bags, 4 muted yellow bags.
            //bright white bags contain 1 shiny gold bag.
            //muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
            //shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
            //dark olive bags contain 3 faded blue bags, 4 dotted black bags.
            //vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
            //faded blue bags contain no other bags.
            //dotted black bags contain no other bags.";

            //            var reader = new StringReader(testInput);

            //            const string testInput2 = @"shiny gold bags contain 2 dark red bags.
            //dark red bags contain 2 dark orange bags.
            //dark orange bags contain 2 dark yellow bags.
            //dark yellow bags contain 2 dark green bags.
            //dark green bags contain 2 dark blue bags.
            //dark blue bags contain 2 dark violet bags.
            //dark violet bags contain no other bags.";

            //            using var reader = new StringReader(testInput2);

            using var stream = File.OpenRead("input.txt");
            using var reader = new StreamReader(stream);
            var bags = BagParser.Parse(reader);

            Console.WriteLine("Bags that can hold 'shiny gold bag': {0}", bags.Values.Count(x => x.CanContain("shiny gold bag")));
            Console.WriteLine("Bags required in shiny gold bag: {0}", bags["shiny gold bag"].CountChildBags());
        }
    }

    public class BagParser
    {
        public static Dictionary<string, Bag> Parse(TextReader reader)
        {
            var dict = new Dictionary<string, Bag>();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var tokens = line.Split(" contain ");

                Bag currentBag = GetBag(tokens[0]);

                if (tokens[1] != "no other bags.")
                {
                    var bags = tokens[1].Split(',');

                    foreach (var bag in bags)
                    {
                        var bagTokens = bag.Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);

                        int count = int.Parse(bagTokens[0]);

                        currentBag.AddBag(GetBag(bagTokens[1]), count);
                    }
                }
            }

            return dict;

            Bag GetBag(string bagName)
            {
                bagName = Bag.Depluralize(bagName);
                if (!dict.ContainsKey(bagName))
                {
                    dict[bagName] = new Bag(bagName);
                }

                return dict[bagName];
            }
        }
    }

    public class Bag
    {
        private readonly string _name;

        private readonly Dictionary<Bag, int> _bagsItCanHold;

        public Bag(string name)
        {
            _name = Depluralize(name);
            _bagsItCanHold = new Dictionary<Bag, int>();
        }

        public override string ToString()
        {
            return _name;
        }

        public void AddBag(Bag bag, int count)
        {
            _bagsItCanHold.Add(bag, count);
        }

        public int CountChildBags()
        {
            if (_bagsItCanHold.Count == 0)
            {
                return 0;
            }

            return _bagsItCanHold.Sum(x => x.Value) + _bagsItCanHold.Sum(x => x.Value * x.Key.CountChildBags());
        }

        public bool CanContain(string bagName)
        {
            foreach (var bag in _bagsItCanHold.Keys)
            {
                if (bag._name == bagName)
                {
                    return true;
                }

                if (bag.CanContain(bagName))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Depluralize(string bagName)
        {
            bagName = bagName.Trim();

            if (bagName.EndsWith('.'))
            {
                bagName = bagName[0..^1];
            }

            if (bagName.EndsWith('s'))
            {
                return bagName[0..^1];
            }
            else
            {
                return bagName;
            }
        }
    }
}
