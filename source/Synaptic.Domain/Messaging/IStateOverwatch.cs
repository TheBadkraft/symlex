
namespace Synaptic.Core;

/// <summary>
/// State overwatch observer interface.
/// </summary>
public interface IStateOverwatch : ISynapticService
{
    /// <summary>
    /// Notify observers of a state change.
    /// </summary>
    void Notify();
    /// <summary>
    /// Subscribe to State Overwatch to receive notifications of state changes.
    /// </summary>
    /// <param name="observer">The observer to subscribe</param>
    /// <returns>An IDisposable object that can be used to unsubscribe from the SynapticHub</returns>
    IDisposable Subscribe(IObserver<IStateChange> observer);
    /// <summary>
    /// Check the current <see cref="SynapticHub.SystemState"/> of the notifier.
    /// </summary>
    /// <typeparam name="TResult">The type of the result</typeparam>
    /// <param name="selector">The selector function to apply to the state</param>
    /// <returns>The result of the selector function</returns>
    TResult GetState<TResult>(Func<ISystemState, TResult> getState);
}
