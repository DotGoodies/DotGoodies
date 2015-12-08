using System;
using System.Collections.Generic;
using System.Threading;
using DotGoodies.Basics;

namespace DotGoodies.Threading
{
    public sealed class StopPhaseWorkCounter : IWorkCount
    {
        private readonly List<StopPhaseWorkCounter> _children = new List<StopPhaseWorkCounter>();

        private readonly Thread _ownerThread;

        private int _activeWorkCount;
        private int _stopRequestsCount;

        public bool IsStopped => Volatile.Read(ref _stopRequestsCount) > 0;

        public static StopPhaseWorkCounter CreateRoot()
        {
            return new StopPhaseWorkCounter(Thread.CurrentThread);
        }

        private StopPhaseWorkCounter(Thread ownerThread)
        {
            _ownerThread = ownerThread.NotNull(nameof(ownerThread));
        }

        private StopPhaseWorkCounter(StopPhaseWorkCounter parent, Thread creatingThread)
            :this(Thread.CurrentThread)
        {
            if(!ReferenceEquals(_ownerThread, creatingThread))
                throw new ArgumentException(
                    $"Initialization of a child counter should happen in the same thread '{_ownerThread.Name}' as parent '{creatingThread.Name}'.");

            _ownerThread = creatingThread;
            parent._children.Add(this);
        }

        public IDisposable WorkStarted()
        {
            var incremented = Interlocked.Increment(ref _activeWorkCount);

            if (incremented < 1)
                throw new InvalidOperationException("Race condition: work counter was incremented from a negative value.");

            return OnDisposeAction.For(WorkFinished);
        }

        public IWorkCount CreateChild()
        {
            return new StopPhaseWorkCounter(this, Thread.CurrentThread);
        }
        private void WorkFinished()
        {
            if (Interlocked.Decrement(ref _activeWorkCount) < 0)
                throw new InvalidOperationException("Race condition: work counter was decremented to a negative value.");
        }

        public void Stop()
        {
            if (Interlocked.Increment(ref _stopRequestsCount) != 1)
                throw new InvalidOperationException("Already stopped.");

            WaitForIdle();

            _children.ForEach(x => x.Stop());
        }

        private void WaitForIdle()
        {
            while (true)
            {
                if (Volatile.Read(ref _activeWorkCount) == 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(30));
                    if (Volatile.Read(ref _activeWorkCount) == 0)
                    {
                        return;
                    }
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }
        }
    }
}