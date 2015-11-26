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
    }
}
