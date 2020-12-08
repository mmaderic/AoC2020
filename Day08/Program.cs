using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static string[] Input;
        static (string Command, int Value)[] Parsed;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var (one, two) = Task();
            var (miliseconds, seconds) = Benchmark.Execute(() => Task());

            Console.WriteLine($"{one}, {two}");
            Console.WriteLine($"{miliseconds}, {seconds}"); // ~6000 ticks
        }

        static (int, int) Task()
        {
            Parsed = Input.Select(x => Parse(x)).ToArray();
            var (acc, _) = Loop();
            var two = PartTwo();

            return (acc, two);
        }

        static int PartTwo()
        {
            var jmps = Parsed.Select((x, i) => (x, i)).Where(x => x.x.Command == "jmp").ToArray();
            (int Acc, bool Valid) result = default;

            foreach (var (_, index) in jmps)
            {
                Parsed[index].Command = "nop";
                result = Loop();
                if (result.Valid)
                    break;

                Parsed[index].Command = "jmp";
            }          

            return result.Acc;
        }

        static (int Acc, bool Valid) Loop()
        {
            var acc = 0;
            var valid = true;
            var map = new bool[Parsed.Length];

            for (var i = 0; i < Parsed.Length; i++)
            {
                if (map[i])
                {
                    valid = false;
                    break;
                }

                map[i] = true;
                switch (Parsed[i].Command)
                {
                    case "nop":
                        continue;
                    case "acc":
                        acc += Parsed[i].Value;
                        continue;
                    case "jmp":
                        i += Parsed[i].Value - 1;
                        continue;
                }
            }

            return (acc, valid);
        }

        static (string Command, int Value) Parse(string line)
        {
            var parts = line.Split(" ");
            var num = int.Parse(parts[1]);

            return (parts[0], num);
        }
    }
}
