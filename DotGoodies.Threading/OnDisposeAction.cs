using System;

namespace DotGoodies.Threading
{
    //TODO - find a way to share this little things without copy them averywhere.
    internal sealed class OnDisposeAction : IDisposable
    {
        public static IDisposable For(Action action)
        {
            return new OnDisposeAction(action);
        }

        private readonly Action _action;

        private OnDisposeAction(Action action)
        {
            _action = action;
        }

        void IDisposable.Dispose()
        {
            _action();
        }
    }
}
