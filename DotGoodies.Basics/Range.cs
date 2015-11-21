using System;

namespace DotGoodies.Basics
{
    public struct Range<T> 
        where T : struct, IComparable<T>, IEquatable<T>
    {
        private readonly T _high;
        private readonly T _low;

        public Range(T low, T high)
        {
            if (low.CompareTo(high) > 0)
            {
                throw new ArgumentException(
                    "Low must be less than or equal to high. Low: {0}, High: {1}".FormatWith(low, high));
            }

            _low = low;
            _high = high;
        }

        public T Low => _low;

        public T High => _high;
    }
}