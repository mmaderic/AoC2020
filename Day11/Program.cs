using AoC2020.Benchmark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static string[] Input;

        static void Main(string[] args)
        {
            Input = File.ReadLines("Input.txt").ToArray();

            var one = Task(IndexesA);
            var two = Task(IndexesB);
            var (miliseconds, ticks) = Benchmark.Execute(() => { Task(IndexesA); Task(IndexesB); });

            Console.WriteLine($"{one}, {two}");
            Console.WriteLine($"{miliseconds}, {ticks}"); // ~11800 ms
        }

        static int Task(Func<string[], int, List<(int, int)>> indexes)
        {
            var seats = Input.Select(x => x.Replace("L", "#")).ToArray();
            while (true)
            {
                if (Empty(seats, indexes))
                    break;

                if (Occupy(seats, indexes))
                    break;
            }

            return seats.SelectMany(x => x).Count(x => x == '#');
        }

        static bool Occupy(string[] seats, Func<string[], int, List<(int, int)>> indexesFunc)
        {
            var indexes = indexesFunc.Invoke(seats, 1);
            if (indexes.Count == 0)
                return true;

            for (var i = 0; i < seats.Length; i++)
            {
                var arr = seats[i].ToCharArray();
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[j] == 'L' && indexes.Any(x => x.Item1 == i && x.Item2 == j))
                        arr[j] = '#';
                }

                seats[i] = new string(arr);
            }

            return false;
        }

        static bool Empty(string[] seats, Func<string[], int, List<(int, int)>> indexesFunc)
        {
            var indexes = indexesFunc.Invoke(seats, 0);
            if (indexes.Count == 0)
                return true;

            foreach (var index in indexes)
            {
                var arr = seats[index.Item1].ToCharArray();
                arr[index.Item2] = 'L';

                seats[index.Item1] = new string(arr);
            }

            return false;
        }

        static List<(int, int)> IndexesA(string[] seats, int mode)
        {
            var indexes = new List<(int, int)>();
            var n = seats.Length - 1;

            for (var i = 0; i < seats.Length; i++)
            {
                var m = seats[i].Length - 1;
                for (var j = 0; j < seats[i].Length; j++)
                {
                    if (seats[i][j] == '.' || (mode == 0 && seats[i][j] == 'L') || (mode == 1 && seats[i][j] == '#'))
                        continue;

                    var ul = i != 0 && j != 0 ? seats[i - 1][j - 1] : '0';
                    var ur = i != 0 && j != m ? seats[i - 1][j + 1] : '0';
                    var dl = i != n && j != 0 ? seats[i + 1][j - 1] : '0';
                    var dr = i != n && j != m ? seats[i + 1][j + 1] : '0';

                    var u = i != 0 ? seats[i - 1][j] : '0';
                    var d = i != n ? seats[i + 1][j] : '0';
                    var l = j != 0 ? seats[i][j - 1] : '0';
                    var r = j != m ? seats[i][j + 1] : '0';

                    var c =  new[] { ul, ur, dl, dr, u, d, l, r }.Count(x => x == '#');
                    if (mode == 0 && c >= 4) indexes.Add((i, j));
                    else if(mode == 1 && c == 0) indexes.Add((i, j));
                }
            }

            return indexes;
        }

        static List<(int, int)> IndexesB(string[] seats, int mode)
        {
            var indexes = new List<(int, int)>();
            var n = seats.Length;

            for (var i = 0; i < n; i++)
            {
                var m = seats[i].Length;
                for (var j = 0; j < m; j++)
                {
                    if (seats[i][j] == '.' || (mode == 0 && seats[i][j] == 'L') || (mode == 1 && seats[i][j] == '#'))
                        continue;

                    var c = 0;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        if (seats[k][j] == '.')
                            continue;

                        if (seats[k][j] == '#')
                        {
                            c++;
                            break;
                        }
                        else break;
                    }

                    for (var k = j - 1; k >= 0; k--)
                    {
                        if (seats[i][k] == '.')
                            continue;

                        if (seats[i][k] == '#')
                        {
                            c++;
                            break;
                        }
                        else break;
                    }

                    for (var k = j + 1; k < m; k++)
                    {
                        if (seats[i][k] == '.')
                            continue;

                        if (seats[i][k] == '#')
                        {
                            c++;
                            break;
                        }
                        else break;
                    }

                    for (var k = i + 1; k < n; k++)
                    {
                        if (seats[k][j] == '.')
                            continue;

                        if (seats[k][j] == '#')
                        {
                            c++;
                            break;
                        }
                        else break;
                    }

                    var d = 0;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        var breakme = false;
                        for (var l = j - 1 - d; l >= 0; l--)
                        {
                            if (seats[k][l] == '.')
                                break;

                            if (seats[k][l] == '#')
                            {
                                c++;
                                breakme = true;
                                break;
                            }
                            else
                            {
                                breakme = true;
                                break;
                            }
                        }

                        if (breakme) break;
                        d++;
                    }

                    d = 0;
                    for (var k = i - 1; k >= 0; k--)
                    {
                        var breakme = false;
                        for (var l = j + 1 + d; l < m; l++)
                        {
                            if (seats[k][l] == '.')
                                break;

                            if (seats[k][l] == '#')
                            {
                                c++;
                                breakme = true;
                                break;
                            }
                            else
                            {
                                breakme = true;
                                break;
                            }
                        }

                        if (breakme) break;
                        d++;
                    }

                    d = 0;
                    for (var k = i + 1; k < n; k++)
                    {
                        var breakme = false;
                        for (var l = j - 1 - d; l >= 0; l--)
                        {
                            if (seats[k][l] == '.')
                                break;

                            if (seats[k][l] == '#')
                            {
                                c++;
                                breakme = true;
                                break;
                            }
                            else
                            {
                                breakme = true;
                                break;
                            }
                        }

                        if (breakme) break;
                        d++;
                    }

                    d = 0;
                    for (var k = i + 1; k < n; k++)
                    {
                        var breakme = false;
                        for (var l = j + 1 + d; l < m; l++)
                        {
                            if (seats[k][l] == '.')
                                break;

                            if (seats[k][l] == '#')
                            {
                                c++;
                                breakme = true;
                                break;
                            }
                            else
                            {
                                breakme = true;
                                break;
                            }
                        }

                        if (breakme) break;
                        d++;
                    }

                    if (mode == 0 && c >= 5) indexes.Add((i, j));
                    else if (mode == 1 && c == 0) indexes.Add((i, j));
                }
            }

            return indexes;
        }        
    }
}
