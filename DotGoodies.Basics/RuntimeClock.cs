using System;

namespace DotGoodies.Basics
{
    public sealed class RuntimeClock : IClock
    {
        public static readonly IClock Instance = new RuntimeClock();

        private RuntimeClock(){}

        public DateTime UtcNow => DateTime.UtcNow;
    }
}