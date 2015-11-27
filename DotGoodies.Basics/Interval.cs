using System;

namespace DotGoodies.Basics
{
    public sealed class Interval<T> 
        where T : IComparable<T>
    {
        public Interval(T low, T high)
        {
            Low = low;
            High = high;
            IsNegative = low.CompareTo(high) > 0;
        }

        public bool IsNegative { get; }

        public T Low { get; }

        public T High { get; }

        public Interval<T> Absolute()
        {
            return IsNegative 
                ? new Interval<T>(High, Low) 
                : this;
        }
    }
}