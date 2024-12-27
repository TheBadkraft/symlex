using Synaptic.IO;

namespace Synaptic.Core;

/// <summary>
/// Represents the main runtime of the Synaptic terminal.
/// </summary>
public interface IRuntime : IDisposable
{
    /// <summary>
    /// Gets the runtime terminal.
    /// </summary>
    ITerminal Terminal { get; }

    /// <summary>
    /// Initializes the runtime.
    /// </summary>
    void Initialize(IServiceContainer services, IResourceService resources);
    /// <summary>
    /// Requests the runtime to shutdown.
    /// </summary>
    void RequestShutdown();
}
