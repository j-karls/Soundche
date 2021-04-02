using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soundche.Core.Domain
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

        public static string Repeat(this string s, int count)
        {
            var _s = new System.Text.StringBuilder().Insert(0, s, count).ToString();
            return _s;
        }
        
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? System.Linq.Enumerable.Empty<T>();
        }

        public static void ReplaceFirst<T>(this List<T> lst, Predicate<T> predicate, T newItem)
        {
            lst[lst.FindIndex(predicate)] = newItem;
        }
    }
}
