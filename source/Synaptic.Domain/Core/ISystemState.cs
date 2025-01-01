
namespace Synaptic.Core;

/// <summary>
/// Interface to interact with the system state.
/// </summary>
public interface ISystemState
{
    /// <summary>
    /// Set the state of the system.
    /// </summary>
    /// <typeparam name="TState">The type of the state</typeparam>
    /// <param name="state">The state to set</param>
    void SetState<TState>(TState state) where TState : Enum;
    /// <summary>
    /// Check if the system is in the specified state.
    /// </summary>
    /// <typeparam name="TState">The type of the state</typeparam>
    /// <param name="state">The state to check</param>
    /// <returns>TRUE if the system is in the specified state; otherwise FALSE</returns>
    bool Is<TState>(TState state) where TState : Enum;
    /// <summary>
    /// Get the current state of the system.
    /// </summary>
    /// <typeparam name="TState">The type of the state</typeparam>
    /// <returns>The current state.</returns>
    TState GetState<TState>() where TState : Enum;
}
