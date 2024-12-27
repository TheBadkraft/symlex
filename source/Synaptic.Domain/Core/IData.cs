namespace Synaptic.Core;

public interface IData<TData> : IEnumerable<TData>, IEnumerator<TData>
{
    /// <summary>
    /// Gets the data object at the specified index.
    /// </summary>
    /// <param name="index">The index</param>
    /// <returns>A data object</returns>
    TData this[int index] { get; }
    /// <summary>
    /// Gets the data list count.
    /// </summary>
    int Count { get; }
}