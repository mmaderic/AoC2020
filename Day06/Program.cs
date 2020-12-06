using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        static string[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var (anyone, everyone) = Loop();
            var (miliseconds, ticks) = Benchmark.Execute(() => Loop());

            Console.WriteLine($"{ anyone}, { everyone}");
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~2 ms
        }

        static (int, int) Loop()
        {
            var anyone = 0;
            var everyone = 0;
            var start = 0;

            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == string.Empty)
                {
                    var answers = string.Join(string.Empty, Input[start..i]);
                    var persons = i - start;

                    anyone += answers.Distinct().Count();
                    everyone += answers.GroupBy(x => x).Where(x => x.Count() == persons).Count();
                    start = i + 1;
                }
            }

            return (anyone, everyone);
        }
    }   
}
