using System;
using System.Collections.Generic;

namespace PrimeNumberLib
{
    public static class PrimeTester
    {
        public static bool IsPrime(long number)
        {
            switch (number)
            {
                case <= 1:
                    return false;
                case 2:
                    return true;
            }

            if (number % 2 == 0) return false;

            var boundary = (long) Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        public static List<long> FormArray(long leftLim, long rightLim)
        {
            var longList = new List<long>();
            if (rightLim <= leftLim)
                throw new ArgumentException("Right limit couldn't be less or equal to left limit");
            for (var i = leftLim; i < rightLim; i++) longList.Add(i);

            return longList;
        }
    }

}