using System;
using System.Collections.Generic;

namespace DotGoodies.Basics
{
    public static class CollectionOfDateTimeExtensions
    {
        public static IEnumerable<TimeSpan> GetDurations(this IReadOnlyList<DateTime> timestamps, bool absolute)
        {
            if(timestamps.Count < 2)
                throw new InvalidOperationException("Insufficient data.");

            for (int i = 0; i < timestamps.Count - 1; i++)
            {
                var duration = timestamps[i + 1] - timestamps[i];
                if (absolute)
                    yield return duration.Duration();
                else
                    yield return duration;
            }
        } 
    }
}
