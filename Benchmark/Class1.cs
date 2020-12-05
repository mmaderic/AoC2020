using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace AoC2020.Benchmark
{
    public static class Benchmark
    {
        public static (double miliseconds, double ticks) Execute(Action action)
        {
            // prevent other threads from interupting
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // warmup
            var watch = new Stopwatch();
            watch.Start();
            while (watch.ElapsedMilliseconds < 1200)
                action.Invoke();
            watch.Stop();

            // execute 20 tests
            var times = new List<(long, long)>();
            for (int repeat = 0; repeat < 20; ++repeat)
            {
                watch.Reset();
                watch.Start();
                action.Invoke();
                watch.Stop();

                times.Add((watch.ElapsedMilliseconds, watch.ElapsedTicks));
            }

            return (times.Average(x => x.Item1), times.Average(x => x.Item2));
        }
    }
}
