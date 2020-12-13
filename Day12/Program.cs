using AoC2020.Benchmark;
using System;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static (char Code, int Value)[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").Select(x => (x[0], Convert.ToInt32(new string(x.Skip(1).ToArray())))).ToArray();

            var one = PartOne();
            var two = PartTwo();
            var (miliseconds, seconds) = Benchmark.Execute(() => { PartOne(); PartTwo(); });

            Console.WriteLine($"{one}, {two}");
            Console.WriteLine($"{miliseconds}, {seconds}"); // ~700 ticks
        }

        static int PartOne()
        {
            var D = new int[] { 0, 0, 0, 0 };
            var F = 0;

            void Move(int index, int value)
            {
                index = ((index % 4) + 4) % 4;
                var opposite = (index + 2) % 4;
                var add = value - D[opposite];
                var sub = value - add;

                D[index] += add;
                D[opposite] -= sub;
            }

            foreach (var (Code, Value) in Input)
            {
                switch (Code)
                {
                    case 'N':
                        Move(3, Value);
                        break;
                    case 'S':
                        Move(1, Value);
                        break;
                    case 'E':
                        Move(0, Value);
                        break;
                    case 'W':
                        Move(2, Value);
                        break;
                    case 'L':
                        F -= Value / 90;
                        break;
                    case 'R':
                        F += Value / 90;
                        break;
                    case 'F':
                        Move(F, Value);
                        break;
                }
            }

            return D.Sum();
        }

        static int PartTwo()
        {
            var D1 = new int[] { 10, 0, 0, 1 };
            var D2 = new int[] { 0, 0, 0, 0 };
            int F1 = 0, F2 = 3;

            void MoveShip(int value)
            {
                var a = ((F1 % 4) + 4) % 4;
                var b = ((F2 % 4) + 4) % 4;
                var oa = (a + 2) % 4;
                var ob = (b + 2) % 4;

                var va = value * D1[a];
                var vb = value * D1[b];
                var adda = va - D2[oa];
                var addb = vb - D2[ob];
                var suba = D2[oa] - va;
                var subb = D2[ob] - vb;

                adda = adda < 0 ? 0 : adda;
                addb = addb < 0 ? 0 : addb;
                suba = suba < 0 ? D2[oa] : va;
                subb = subb < 0 ? D2[ob] : vb;

                D2[a] += adda;
                D2[b] += addb;
                D2[oa] -= suba;
                D2[ob] -= subb;
            }

            void MoveWaypoint(int index, int value)
            {
                index = ((index % 4) + 4) % 4;
                var opposite = (index + 2) % 4;

                var add = value - D1[opposite];
                add = add < 0 ? 0 : add;

                var sub = D1[opposite] - value;
                sub = sub < 0 ? D1[opposite] : value;

                D1[index] += add;
                D1[opposite] -= sub;

                F1 = D1[0] > 0 ? 0 : 2;
                F2 = D1[1] > 0 ? 1 : 3;
            }

            void RotateWaypoint(int n, int direction)
            {
                for (int j = 0; j < n; j++)
                {
                    if (direction == -1)
                    {
                        var tmp = D1[0];
                        for (var i = 0; i < 3; i++)
                            D1[i] = D1[i + 1];
                        D1[3] = tmp;
                        F1--;
                        F2--;
                    }
                    else
                    {
                        var tmp = D1[3];
                        for (var i = 3; i > 0; i--)
                            D1[i] = D1[i - 1];
                        D1[0] = tmp;
                        F1++;
                        F2++;
                    }
                }
            }

            foreach (var (Code, Value) in Input)
            {
                switch (Code)
                {
                    case 'N':
                        MoveWaypoint(3, Value);
                        break;
                    case 'S':
                        MoveWaypoint(1, Value);
                        break;
                    case 'E':
                        MoveWaypoint(0, Value);
                        break;
                    case 'W':
                        MoveWaypoint(2, Value);
                        break;
                    case 'L':
                        RotateWaypoint(Value / 90, -1);
                        break;
                    case 'R':
                        RotateWaypoint(Value / 90, +1);
                        break;
                    case 'F':
                        MoveShip(Value);
                        break;
                }
            }

            return D2.Sum();
        }
    }
}
