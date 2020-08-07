﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public static class ExtensionMethods
    {
        public static IEnumerable<double> CumulativeSum(this IEnumerable<double> sequence)
        {
            double sum = 0;
            foreach (var item in sequence)
            {
                sum += item;
                yield return sum;
            }
        }
    }
}
