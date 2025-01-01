
namespace Synaptic.Core;

/// <summary>
/// Represents a service in the Synaptic system.
/// </summary>
public interface ISynapticService
{
    /// <summary>
    /// Called when the service is registered with the <see cref="IServiceContainer"/>.
    /// </summary>
    /// <param name="serviceContainer">The service container.</param>
    void OnRegistered(IServiceContainer serviceContainer);
    /// <summary>
    /// Starts the service.
    /// </summary>
    void Start();
    /// <summary>
    /// Stops the service.
    /// </summary>
    void Stop();
}