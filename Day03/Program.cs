using System;
using System.Diagnostics;
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

            var watch = new Stopwatch();
            watch.Start();

            var a = Loop(1, 1);
            var b = Loop(3, 1);
            var c = Loop(5, 1);
            var d = Loop(7, 1);
            var e = Loop(1, 2);

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds); // ~0 ms

            Console.WriteLine((long)a * b * c * d * e);
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
