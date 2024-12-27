
namespace Synaptic.Core;

public partial class SynapticHub
{
    private class StateOverwatch : IStateOverwatch, IStateChange
    {
        private static List<IObserver<IStateChange>> Observers { get; } = new();
        private SystemState State { get; init; }

        internal StateOverwatch(SystemState systemState) => State = systemState;

        /// <summary>
        /// Notify all observers of a state change.
        /// </summary>
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(this);
            }
        }
        /// <summary>
        /// </inheritdoc>
        /// </summary>
        public IDisposable Subscribe(IObserver<IStateChange> observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }

            return new Unsubscriber(observer);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TResult GetState<TResult>(Func<ISystemState, TResult> getState) => getState(State);

        public void OnRegistered(IServiceContainer serviceContainer)
        {
            //  nothing to do here ...
        }

        /// <summary>
        /// Overwatch Unsuscriber class.
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private IObserver<IStateChange> Observer { get; init; }

            /// <summary>
            /// Create a new Unsubscriber for the given observer.
            /// </summary>
            /// <param name="observer">The observer to unsubscribe</param>
            internal Unsubscriber(IObserver<IStateChange> observer)
            {
                Observer = observer;
            }

            public void Dispose()
            {
                if (Observers.Contains(Observer))
                {
                    Observers.Remove(Observer);
                }
            }
        }

    }

    /// <summary>
    /// Update the state of the SynapticHub and notify observers of the change.
    /// </summary>
    /// <param name="update">The update function to apply to the state</param>
    public void UpdateState(Action<ISystemState> update)
    {
        update(State);
        Overwatch.Notify();
    }
    /// <summary>
    /// Check the current <see cref="SynapticHub.SystemState"/> of the notifier.
    /// </summary>
    /// <typeparam name="TResult">The type of the result</typeparam>
    /// <param name="selector">The selector function to apply to the state</param>
    /// <returns>The result of the selector function</returns>
    public TResult GetState<TResult>(Func<ISystemState, TResult> getState)
    {
        return getState(State);
    }
    /// <summary>
    /// Subscribe to State Overwatch to receive notifications of state changes.
    /// </summary>
    /// <param name="observer">The observer to subscribe</param>
    /// <returns>An IDisposable object that can be used to unsubscribe from the SynapticHub</returns>
    public IDisposable Subscribe(IObserver<IStateChange> observer)
    {
        return Overwatch.Subscribe(observer);
    }
}
