
namespace Synaptic.Core;

/// <summary>
/// State change interface.
/// </summary>
public interface IStateChange
{
    /// <summary>
    /// Check the state of the system.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="stateFunc">The function to get the state.</param>
    /// <returns>The state of the system.</returns>
    TResult CheckState<TResult>(Func<ISystemState, TResult> stateFunc);
}
