using System;
using System.Collections.Generic;

namespace Marge.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> f)
        {
            foreach (var x in source)
            {
                f(x);
            }
        }
    }
}