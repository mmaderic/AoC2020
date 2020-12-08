using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static string[] Input;
        static (string Bag, (string Bag, int Count)[] Items)[] Parsed;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();
            Parsed = Input.Select(x => Parse(x)).ToArray();

            var one = PartOne();
            var two = PartTwo("shiny gold") - 1;            
            var (miliseconds, seconds) = Benchmark.Execute(() => { Input.Select(x => Parse(x)).ToArray(); PartOne(); PartTwo("shiny gold"); });

            Console.WriteLine($"{one}, {two}");
            Console.WriteLine($"{miliseconds}, {seconds}"); // ~ 4.5 ms
        }

        static int PartOne()
        {
            var union = Parsed.Where(x => x.Items.Any(y => y.Bag == "shiny gold")).Select(x => x.Bag).ToArray();
            var previous = union;

            while (true)
            {
                var layer = Parsed.Where(x => x.Items.Any(y => previous.Contains(y.Bag))).Select(x => x.Bag).Except(union).ToArray();
                if (layer.Length == 0)
                    break;

                union = union.Union(layer).ToArray();
                previous = layer;
            }

            return union.Length;
        }

        static int PartTwo(string bag)
        {
            if (bag == "no other")
                return 1;

            var prada = Parsed.First(x => x.Bag == bag);
            var count = 1;

            foreach (var item in prada.Items)            
                count += item.Count * PartTwo(item.Bag);

            return count;
        }

        static (string Bag, (string Bag, int Count)[] Items) Parse(string line)
        {
            var parts = line.Split("contain");
            var bag = parts[0].Replace(" bags ", string.Empty);
            var contains = parts[1].Split(',').Select(x =>
            {
                var bag = x[^1] == '.' ? x.Remove(x.Length - 1) : x;
                var count = x.Contains("no other bags") ? 0 : x.First(y => char.IsDigit(y)) - '0';
                bag = bag.Replace(count.ToString(), string.Empty);
                bag = (count == 1 ? bag.Replace("bag", string.Empty) : bag.Replace("bags", string.Empty)).TrimStart().TrimEnd();

                return (bag, count);
            }).ToArray();

            return (bag, contains);
        }
    }
}
