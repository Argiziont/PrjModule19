using PrimeNumberLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace PrjModule19
{
    internal static class Program
    {
        private static void Main()
        {
            var formedArray = PrimeTester.FormArray(0, 100);
            var stopwatch = Stopwatch.StartNew();
            var task1 = Task1(formedArray);
            while (!task1)
            {

            }
            stopwatch.Stop();
            Console.WriteLine($"Thread 1 completed in {stopwatch.ElapsedMilliseconds}");

            formedArray = PrimeTester.FormArray(0, 100);
            stopwatch = Stopwatch.StartNew();
            var task2 = Task2(formedArray);
            while (!task2)
            {

            }
            stopwatch.Stop();
            Console.WriteLine($"Thread 2 completed in {stopwatch.ElapsedMilliseconds}");
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
                            //Console.WriteLine(PrimeTester.IsPrime(number) ? $"{number} is prime" : $"{number} isn't prime");
                        }
                    }
                });
                testedThreadArray.Add(subThread);
                subThread.Start();
            }
            while (testedThreadArray.FindIndex(t => t.ThreadState != ThreadState.Stopped)!=-1)
            {
            }
            return true;
        }
        private static bool Task2(IList<long> testedArray)
        {
            var testedThreadNumber = testedArray.Count / 10;
            var testedThreadArray = new List<Thread>();

            object locker = new();

            var rng = new Random();

            for (var i = 0; i < testedThreadNumber; i++)
                if (testedArray.Count > 0)
                {
                    while (testedArray.Count >= 1)
                    {
                        var subThread = new Thread(() =>
                        {
                            if (testedArray.Count == 0)
                                return;

                            lock (locker)
                            {
                                var number = testedArray[rng.Next(testedArray.Count)];
                                testedArray.Remove(number);
                                //Console.WriteLine(PrimeTester.IsPrime(number)
                                //    ? $"{number} is prime"
                                //    : $"{number} isn't prime");
                            }

                        });
                        subThread.Start();
                        testedThreadArray.Add(subThread);
                    }
                }
                else
                    break;



            while (testedThreadArray.FindIndex(t => t.ThreadState != ThreadState.Stopped) != -1)
            {
            }
            return true;
        }

        private static IEnumerable<List<T>> SplitList<T>(List<T> list, int nSize)
        {
            for (var i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
    }
}