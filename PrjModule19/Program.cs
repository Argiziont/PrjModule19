using PrimeNumberLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace PrjModule19
{
    internal static class Program
    {
        private static void Main()
        {
            //var formedArray = PrimeTester.FormArray(0, 100);
            //var task1 = Task1(formedArray);

            var formedArray = PrimeTester.FormArray(0, 10);
            Task.WaitAll(Task2(formedArray));
        }

        private static bool Task1(List<long> testedArray)
        {
            var parts = testedArray.Count / 10;
            var splitedLists = SplitList(testedArray, parts);
            var testedThreadArray = new List<Thread>();
            object locker = new();
            var rng = new Random();

            foreach (var partedList in splitedLists)
            {
                var subThread = new Thread(() =>
                {
                    while (true)
                    {
                        if (partedList.Count == 0)
                            return;

                        lock (locker)
                        {
                            var number = partedList[rng.Next(partedList.Count)];
                            partedList.Remove(number);
                            Console.WriteLine(PrimeTester.IsPrime(number)
                                ? $"{number} is prime"
                                : $"{number} isn't prime");
                        }
                    }
                });
                testedThreadArray.Add(subThread);
                subThread.Start();
            }

            while (testedThreadArray.FindIndex(t => t.ThreadState != ThreadState.Stopped) != -1)
            {
            }

            return true;
        }

        private static Task[] Task2(ConcurrentDictionary<int, long> testedArray)
        {
            var testedThreadArray = new List<Task>();

            var rng = new Random();

            for (var i = 0; i < testedArray.Count; i++)
            {
                var i1 = i;
                var subTask = new Task((() =>
                {
                    if (!testedArray.TryRemove(i1, out var number)) return;
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