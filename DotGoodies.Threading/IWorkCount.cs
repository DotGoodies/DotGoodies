using System;

namespace DotGoodies.Threading
{
    public interface IWorkCount
    {
        bool IsStopped { get; }
        IDisposable WorkStarted();
        IWorkCount CreateChild();
    }
}