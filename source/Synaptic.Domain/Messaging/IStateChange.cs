
namespace Synaptic.Core;

/// <summary>
/// State change interface.
/// </summary>
public interface IStateChange
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="getState"></param>
    /// <returns></returns>
    TResult GetState<TResult>(Func<ISystemState, TResult> getState);
}
