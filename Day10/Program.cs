using AoC2020.Benchmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static int[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").Select(x => Convert.ToInt32(x)).ToArray();

            var (one, two) = Task();
            var (miliseconds, ticks) = Benchmark.Execute(() => Task());

            Console.WriteLine($"{max}, {total}");
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~500 ticks
        }

        static (int, ulong) Task()
        {
            var max = Input.Max() + 3;
            var ordered = Input.Concat(new[] { 0, max, }).OrderBy(x => x).ToArray();
            var value = ordered.Take(ordered.Length - 1)
                .Zip(ordered.Skip(1), (x, y) => y - x)
                .GroupBy(x => x).Select(x => x.Count())
                .Aggregate((x, y) => x * y);


            var values = Enumerable.Range(0, max).Select(x => new List<int>()).ToArray();
            for (var i = 0; i < ordered.Length; i++)
            {
                for (var j = i + 1; j < ordered.Length; j++)
                {
                    if (ordered[j] - ordered[i] > 3)
                        break;

                    values[ordered[i]].Add(ordered[j]);
                }
            }

            var cache = new ulong[max];
            ulong RecursiveCount(int start, int end, ulong count)
            {
                if (start == end)
                    return ++count;
                else if (cache[start] == 0)
                {
                    ulong sum = 0;
                    foreach (var value in values[start])
                        sum += RecursiveCount(value, end, count);

                    cache[start] = sum;
                    return sum;
                }
                else
                    return cache[start];
            }

            var count = RecursiveCount(0, max, 0);
            return (value, count);
        }
    }
}