using AoC2020.Benchmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static int[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").Select(x => Convert.ToInt32(x)).ToArray();            

            var numA = PartOne();
            var numB = PartTwo();

            var (miliseconds, ticks) = Benchmark.Execute(() => { PartOne(); PartTwo(); });

            Console.WriteLine($"{numA}, {numB}");
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~21 ms
        }

        static int PartOne()
        {
            foreach (var valueA in Input)
            {
                var valueB = Input.FirstOrDefault(x => 2020 - valueA == x);
                if (valueB.Equals(default))
                    continue;
                
                return valueA * valueB;
            }

            return default;
        }

        static int PartTwo()
        {
            var items = new List<(int, int, int)>();
            foreach (var valueA in Input)
            {
                foreach (var valueB in Input)
                    items.Add((valueA, valueB, valueA + valueB));
            }

            foreach (var valueC in Input)
            {
                var valueAB = items.FirstOrDefault(x => 2020 - valueC == x.Item3);
                if (valueAB.Equals(default))
                    continue;

                return valueAB.Item1 * valueAB.Item2 * valueC;
            }

            return default;
        }
    }
}
