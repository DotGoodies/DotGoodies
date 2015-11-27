using System;
using System.Threading;

namespace DotGoodies.Threading
{
    public sealed class SingleEntranceTimer : IDisposable
    {
        private const int Idle = 0;
        private const int Working = 1;

        private readonly Action _periodicTimerAction;
        private readonly Timer _timer;
        private readonly TimeSpan _resolution;

        private int _state = Idle;

        public SingleEntranceTimer(Action periodicTimerAction, TimeSpan resolution)
        {
            _periodicTimerAction = () => WrapAction(periodicTimerAction);
            _timer = new Timer(_ => _periodicTimerAction());
            _resolution = resolution;
        }

        public void Start()
        {
            _timer.Change(_resolution, _resolution);
        }

        public void Stop()
        {
            _timer.Change(-1, -1);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void WrapAction(Action action)
        {
            if (Interlocked.CompareExchange(ref _state, Working, Idle) != Idle)
                return;

            try
            {
                action();
            }
            finally
            {
                Interlocked.Exchange(ref _state, Idle);
            }
        }
    }
}
