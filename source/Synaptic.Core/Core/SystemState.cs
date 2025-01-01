
namespace Synaptic.Core;

/// <summary>
/// Retains the state of the system.
/// </summary>
/// <remarks>
/// The <see cref="SystemState"/> class is used to retain the state of the system.
/// </remarks>
public class SystemState : ISystemState
{
    /// <summary>
    /// The current runtime state.
    /// </summary>
    public RuntimeState RuntimeState { get; private set; } = RuntimeState.Idle;
    /// <summary>
    /// The current Synaptic state.
    /// </summary>
    public SynapticState SynapticState { get; private set; } = SynapticState.Idle;

    /// <summary>
    /// Creates a new instance of the <see cref="SystemState"/> class.
    /// </summary>
    public SystemState() { }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public void SetState<TState>(TState state) where TState : Enum
    {
        switch (state)
        {
            case RuntimeState runtimeState:
                RuntimeState = runtimeState;

                break;
            case SynapticState synapticState:
                SynapticState = synapticState;

                break;
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public bool Is<TState>(TState state) where TState : Enum
    {
        switch (state)
        {
            case RuntimeState runtimeState:
                return RuntimeState == runtimeState;

            case SynapticState synapticState:
                return SynapticState == synapticState;

            default:
                return false;
        }
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="TState"><inheritdoc/></typeparam>
    /// <returns><inheritdoc/></returns>
    public TState GetState<TState>() where TState : Enum
    {
        return typeof(TState) switch
        {
            Type t when t == typeof(RuntimeState) => (TState)(RuntimeState as object),
            Type t when t == typeof(SynapticState) => (TState)(SynapticState as object),
            _ => default
        };
    }
}

/// <summary>
/// Represents the state of the runtime.
/// </summary>
public enum RuntimeState
{
    /// <summary>
    /// The runtime is in an idle state.
    /// </summary>
    Idle,
    /// <summary>
    /// The runtime is in a running state.
    /// </summary>
    Running,
    /// <summary>
    /// The runtime is in an error state.
    /// </summary>
    Error,
    /// <summary>
    /// The runtime is in a shutdown state.
    /// </summary>
    Shutdown
}
/// <summary>
/// Represents the state of the Synaptic interpreter.
/// </summary>
public enum SynapticState
{
    /// <summary>
    /// The Synaptic interpreter is in an idle state.
    /// </summary>
    Idle,
    /// <summary>
    /// The Synaptic interpreter is in a running state.
    /// </summary>
    Running,
    /// <summary>
    /// The Synaptic interpreter is in an error state.
    /// </summary>
    Error,
    /// <summary>
    /// The Synaptic interpreter is in a shutdown state.
    /// </summary>
    Shutdown
}
