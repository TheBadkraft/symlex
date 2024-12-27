namespace Synaptic.Core;

public interface IServiceContainer : IServiceProvider
{
    #region Service Management
    /// <summary>
    /// Get a service from the service container.
    /// </summary>
    /// <typeparam name="TService">The type of the service to get</typeparam>
    /// <returns>The service</returns>
    TService GetService<TService>() where TService : class, ISynapticService;
    /// <summary>
    /// Register a service with the service container.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register</typeparam>
    /// <returns>The service</returns>
    TService RegisterService<TService>(TService service) where TService : class, ISynapticService;
    #endregion Service Management
}
