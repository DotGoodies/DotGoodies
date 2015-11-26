using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DotGoodies.Threading
{
    internal sealed class ConcurrentQueueProcessor
    {
        private const int DefaultQueueLength = 1024;
        private static readonly Action StopSignal = () => { };

        private readonly Action<Exception> _errorReporter;

        private readonly List<Thread> _threads = new List<Thread>();

        private readonly BlockingCollection<Action> _queue;

        private volatile bool _stopped;

        public ConcurrentQueueProcessor(Action<Exception> errorReporter, int numberOfWorkers)
            : this(errorReporter, numberOfWorkers, DefaultQueueLength)
        { }

        private ConcurrentQueueProcessor(
            Action<Exception> errorReporter,
            int numberOfWorkers,
            int queueLength)
        {
            _errorReporter = errorReporter;

            var processorInstanceName = GetType().Name + "_" + GetHashCode();

            for (int i = 0; i < numberOfWorkers; i++)
            {
                var thread = new Thread(Consume)
                {
                    IsBackground = true,
                    Name = processorInstanceName + "#" + i
                };

                _threads.Add(thread);
            }

            _queue = new BlockingCollection<Action>(new ConcurrentQueue<Action>(), queueLength);
        }

        private void Consume()
        {
            while (true)
            {
                var action = _queue.Take();

                if (action == StopSignal)
                    return;

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    _errorReporter(ex);
                }
            }
        }


        public void Start()
        {
            _threads.ForEach(x => x.Start());
        }

        public void Enqueue(Action action)
        {
            if (_stopped && action != StopSignal)
                return;

            _queue.Add(action);
        }

        public void Stop()
        {
            _stopped = true;

            for (int i = 0; i < _threads.Count; i++)
            {
                Enqueue(StopSignal);
            }

            _threads.ForEach(x => x.Join());
        }
    }
}

