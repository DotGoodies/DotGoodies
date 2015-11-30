using System;
using System.Threading;

namespace DotGoodies.Threading
{
    public sealed class BackgroundWorkersPool
    {
        private static readonly Action<Exception> DefaultExceptionHandler = _ => {}; 

        private readonly int _maxCapacity;

        private int _busyCount;
        private readonly IWorkCount _stopGuard;
        private readonly Action<Exception> _lastChanceExceptionHandler;

        public BackgroundWorkersPool(int maxCapacity, IWorkCount stopGuard) 
            : this(maxCapacity, stopGuard, DefaultExceptionHandler)
        {
            
        }

        public BackgroundWorkersPool(
            int maxCapacity, 
            IWorkCount stopGuard, 
            Action<Exception> lastChanceExceptionHandler)
        {
            _maxCapacity = maxCapacity;
            _lastChanceExceptionHandler = lastChanceExceptionHandler;
            _stopGuard = stopGuard;
        }

        public bool HasCapacity
        {
            get
            {
                if (_stopGuard.IsStopped)
                    return false;

                Interlocked.MemoryBarrier();
                return _busyCount < _maxCapacity;
            }
        }

        public bool TryEnqueue(Action work)
        {
            using (_stopGuard.WorkStarted())
            {
                if (_stopGuard.IsStopped)
                    return false;

                var runningWorks = Interlocked.Increment(ref _busyCount);

                if (runningWorks > _maxCapacity)
                {
                    Interlocked.Decrement(ref _busyCount);

                    throw new InvalidOperationException("The pool is full.");
                }

                var guard = _stopGuard.WorkStarted();

                if (runningWorks == _maxCapacity)
                {
                    ThreadPool.QueueUserWorkItem(_ => RunAction(work, guard.Dispose));
                    return false;
                }

                ThreadPool.QueueUserWorkItem(_ => RunAction(work, guard.Dispose));

                return true;
            }
        }

        private void RunAction(Action work, Action workFinished)
        {
            using (_stopGuard.WorkStarted())
            {
                try
                {
                    work();
                }
                catch (Exception ex)
                {
                    _lastChanceExceptionHandler(ex);
                }
                finally
                {
                    Interlocked.Decrement(ref _busyCount);
                    workFinished();
                }
            }
        }
    }
}
