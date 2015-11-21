using System;
using System.Collections.Generic;

namespace DotGoodies.Basics
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
        {
            int counter = 0;
            foreach (T item in sequence)
            {
                action(counter, item);
                counter++;
            }
        }

        public static bool TryOneByOne<T>(this IEnumerable<T> sequence, Action<T> action,
                                              Action<Exception> errorAction)
        {
            foreach (var item in sequence)
            {
                try
                {
                    action(item);
                    return true;
                }
                catch (Exception ex)
                {
                    errorAction(ex);
                }
            }

            return false;
        }

        public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> sequence, T element)
        {
            yield return element;

            foreach (var item in sequence)
            {
                yield return item;
            }
        }
    }
}