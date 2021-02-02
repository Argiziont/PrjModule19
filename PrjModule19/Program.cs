using PrimeNumberLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrjModule19
{
    internal static class Program
    {
        private static void Main()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var formedArray = PrimeTester.FormArray(0, 10);
            Task.WaitAll(Task1(formedArray));
            stopwatch.Stop();
            Console.WriteLine($"\nTask 1 taken {stopwatch.ElapsedMilliseconds} ms\n");

            stopwatch.Start();
            formedArray = PrimeTester.FormArray(0, 10);
            Task.WaitAll(Task2(formedArray));
            stopwatch.Stop();
            Console.WriteLine($"\nTask 2 taken {stopwatch.ElapsedMilliseconds} ms\n");
        }

        private static Task[] Task1(ConcurrentDictionary<int,long> testedArray)
        {

            var parts = testedArray.Count / 10;
            var splitedLists = SplitList(testedArray.ToList(), parts);


            return splitedLists.Select(partedList => Task.Factory.StartNew((() =>
            {
                foreach (var (_, value) in partedList)
                {
                    Console.WriteLine(PrimeTester.IsPrime(value)
                        ? $"{value} is prime"
                        : $"{value} isn't prime");
                }
            }))).ToArray();
        }

        private static Task[] Task2(ConcurrentDictionary<int, long> testedArray)
        {
            var testedThreadArray = new List<Task>();
            ThreadPool.SetMaxThreads(testedArray.Count / 10, testedArray.Count / 10);

            var rng = new Random();
            var indexes = new HashSet<int>();
            while (indexes.Count < testedArray.Count)
            {
                indexes.Add(rng.Next(0, testedArray.Count));
            }

            for (var i = 0; i < testedArray.Count; i++)
            {
                var i1 = i;
                var subTask = new Task((() =>
                {
                    if (!testedArray.TryRemove(indexes.ToArray()[i1], out var number)) return;
                    Console.WriteLine(PrimeTester.IsPrime(number)
                        ? $"{number} is prime"
                        : $"{number} isn't prime");
                }));
                subTask.Start();
                testedThreadArray.Add(subTask);
            }
            return testedThreadArray.ToArray();
        }
        private static IEnumerable<List<T>> SplitList<T>(List<T> list, int nSize)
        {
            for (var i = 0; i < list.Count; i += nSize) yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
        }
    }
}