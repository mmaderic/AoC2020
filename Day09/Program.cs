using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        static long[] Input;
        static int Preamble = 25;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").Select(x => Convert.ToInt64(x)).ToArray();

            var one = PartOne();
            var two = PartTwo(one);
            var (miliseconds, ticks) = Benchmark.Execute(() => { var one = PartOne(); PartTwo(one); });

            Console.WriteLine($"{one}, {two}");
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~1 ms
        }

        static long PartOne()
        {
            var sums = new long[Preamble * Preamble];
            int start = 0, end = Preamble;

            for (var i = Preamble; i < Input.Length; i++)
            {
                var index = 0;
                for (var j = start; j < end; j++)
                {
                    for (var k = start; k < end; k++)
                        sums[index++] = Input[j] + Input[k];
                }

                if (!sums.Contains(Input[i]))
                    return Input[i];

                start++; end++;
            }

            return default;
        }

        static long PartTwo(long invalid)
        {
            int start = 0;
            while (true)
            {
                long sum = 0;
                var index = start;
                while (sum < invalid)
                {
                    sum += Input[index++];
                    if (sum == invalid)
                    {
                        var range = Input[start..(index - 1)];
                        return range.Min() + range.Max();
                    }
                }

                start++;
            }
        }
    }
}
