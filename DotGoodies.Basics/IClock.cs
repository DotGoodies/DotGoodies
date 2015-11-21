using System;

namespace DotGoodies.Basics
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}