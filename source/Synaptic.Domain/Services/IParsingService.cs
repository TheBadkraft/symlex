
namespace Synaptic.Core;

/// <summary>
/// Represents a service for parsing context descriptors.
/// </summary>
public interface IParsingService : ISynapticService
{
    /// <summary>
    /// Enqueues the data for parsing.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="data">The data to enqueue.</param>
    void Enqueue<TData>(TData data);
}
