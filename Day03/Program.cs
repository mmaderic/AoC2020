using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static string[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();           

            var a = Loop(1, 1);
            var b = Loop(3, 1);
            var c = Loop(5, 1);
            var d = Loop(7, 1);
            var e = Loop(1, 2);

            var (miliseconds, ticks) = Benchmark.Execute(() =>
            {
                Loop(1, 1);
                Loop(3, 1);
                Loop(5, 1);
                Loop(7, 1);
                Loop(1, 2);
            });
          
            Console.WriteLine((long)a * b * c * d * e);
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~87 ticks
        }

        static int Loop(int xOffest, int yOffest)
        {
            var x = 0;
            var y = 0;
            var trees = 0;

            do
            {
                x += xOffest;
                y += yOffest;

                if (Input[y][x] == '#')
                    trees++;

                if (x + xOffest >= Input[y].Length)
                    x = - 1 * xOffest + (x + xOffest - Input[y].Length);

            } while (y + yOffest < Input.Length);

            return trees;          
        }
    }
}
