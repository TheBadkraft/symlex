namespace Synaptic.Core;

/// <summary>
/// SynapticHub interface.
/// </summary>
public partial interface ISynapticHub
{
    /// <summary>
    /// Get the <see cref="IServiceContainer"/>.
    /// </summary>
    IServiceContainer Services { get; }
    /// <summary>
    /// Get the <see cref="IResourceService"/>.
    /// </summary>
    IResourceService Resources { get; }

    #region Runtime Management
    /// <summary>
    /// Register a runtime with the SynapticHub.
    /// </summary>
    /// <param name="runtime">The runtime to register</param>
    void RegisterRuntime(IRuntime runtime);
    #endregion Runtime Management
    #region Overwatch SystemState Management
    /// <summary>
    /// Select state from the <see cref="SystemState"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the result</typeparam>
    /// <param name="getState">The get state selector function to apply to the state</param>
    /// <returns>The result of the selector function</returns>
    TResult GetState<TResult>(Func<ISystemState, TResult> getState);
    /// <summary>
    /// Subscribe to the state overwatch to receive notifications of state changes.
    /// </summary>
    /// <param name="observer">The observer to subscribe</param>
    /// <returns>An IDisposable object that can be used to unsubscribe from the SynapticHub</returns>
    IDisposable Subscribe(IObserver<IStateChange> observer);
    /// <summary>
    /// Update the state of the SynapticHub and notify observers of the change.
    /// </summary>
    /// <param name="value">The update function to apply to the state</param>
    void UpdateState(Action<ISystemState> value);
    #endregion Overwatch SystemState Management
}
