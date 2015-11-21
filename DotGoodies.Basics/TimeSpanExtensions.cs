using System;
using System.Collections.Generic;
using System.Linq;

namespace DotGoodies.Basics
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan MultiplyBy(this TimeSpan value, int times)
        {
            TimeSpan result = TimeSpan.Zero;
            for (int i = 0; i < times; i++)
            {
                result += value;
            }
            return result;
        }

        public static TimeSpan DivideBy(this TimeSpan value, int times)
        {
            long ticks = value.Ticks/times;

            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan AddMilliseconds(this TimeSpan timeSpan, int milliseconds)
        {
            return timeSpan.Add(TimeSpan.FromMilliseconds(milliseconds));
        }

        public static TimeSpan Average(this IEnumerable<TimeSpan> sequence)
        {
            return TimeSpan.FromTicks((long)sequence.Average(x => x.Ticks));
        }

        public static TimeSpan AddSeconds(this TimeSpan span, int seconds)
        {
            return span.Add(TimeSpan.FromSeconds(seconds));
        }
    }
}