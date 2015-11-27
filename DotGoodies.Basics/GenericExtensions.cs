using System.Linq;

namespace DotGoodies.Basics
{
    public static class GenericExtensions
    {
        public static bool Is<T>(this object obj)
        {
            return obj is T;
        }

        public static bool IsNot<T>(this object obj)
        {
            return !obj.Is<T>();
        }

        public static T As<T>(this object obj)
        {
            return (T) obj;
        }

        public static bool IsOneOf<T>(this T obj, params T[] candidates)
        {
            return candidates.Contains(obj);
        }
    }
}
