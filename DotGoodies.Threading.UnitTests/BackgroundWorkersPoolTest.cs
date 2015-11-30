using System;
using System.Threading;
using DotGoodies.Basics;
using NUnit.Framework;

namespace DotGoodies.Threading.UnitTests
{
    [TestFixture]
    public class BackgroundWorkersPoolTest
    {
        private const int MaxCapacity = 2;

        private static readonly TimeSpan ActionDelay = TimeSpan.FromMilliseconds(500);

        private CountdownEvent _actionsCompleteEvent;

        private BackgroundWorkersPool _pool;

        [SetUp]
        public void TestSetUp()
        {
            _pool = new BackgroundWorkersPool(
                MaxCapacity,
                StopPhaseWorkCounter.CreateRoot());

            _actionsCompleteEvent = new CountdownEvent(MaxCapacity);
        }

        [TearDown]
        public void TestTearDown()
        {
            try
            {
                if (!_actionsCompleteEvent.Wait(ActionDelay.MultiplyBy(8)))
                    throw new TimeoutException("One of the actions is not able to finish in time.");
            }
            finally
            {
                _actionsCompleteEvent.Dispose();
            }
        }

        [Test]
        public void Does_not_allow_to_run_more_actions_then_configured()
        {
            AssertEnqueue(true);
            AssertEnqueue(false);
            Assert.Catch<InvalidOperationException>(() => AssertEnqueue(false));
        }

        [Test]
        public void Throttling_works_as_expected_even_when_some_of_the_actions_throw_exceptions()
        {
            AssertEnqueue(true, () => { Thread.Sleep(ActionDelay); throw new Exception(); });
            AssertEnqueue(false);
            Assert.Catch<InvalidOperationException>(() => AssertEnqueue(false));

            Pause();

            _actionsCompleteEvent.Reset();

            AssertEnqueue(true);
            AssertEnqueue(false);
            Assert.Catch<InvalidOperationException>(() => AssertEnqueue(false));
        }

        private void Pause()
        {
            _actionsCompleteEvent.Wait();
            Thread.Sleep(ActionDelay);
        }

        private void AssertEnqueue(bool expectedResult, Action additionalAction = null)
        {
            try
            {
                Assert.AreEqual(
                    expectedResult, 
                    _pool.TryEnqueue(() 
                        => TestAction(additionalAction)));
            }
            catch
            {
                _actionsCompleteEvent.Signal();
                throw;
            }
        }

        private void TestAction(Action additionalAction = null)
        {
            try
            {
                if (additionalAction != null)
                {
                    additionalAction();
                    return;
                }

                Thread.Sleep(ActionDelay);
            }
            finally
            {
                _actionsCompleteEvent.Signal();
            }
        }

    }
}
