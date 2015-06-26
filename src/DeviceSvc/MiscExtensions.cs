using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceSvc
{
    public static class MiscExtensions
    {
        // Ex: collection.TakeLast(5);
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int n)
        { 
            return source.Skip(Math.Max(0, source.Count() - n));
        }
    }
}