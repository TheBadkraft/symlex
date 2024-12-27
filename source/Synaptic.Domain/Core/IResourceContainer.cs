namespace Synaptic.Core;

public interface IResourceService : IResourceProvider, ISynapticService
{
    #region Resource Management
    /// <summary>
    /// Get a resource from the resource container.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource to get</typeparam>
    /// <returns>The resource</returns>
    TResource GetResource<TResource>() where TResource : class;
    /// <summary>
    /// Register a resource with the resource container.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource to register</typeparam>
    /// <returns>The resource</returns>
    TResource RegisterResource<TResource>(TResource resource) where TResource : class;
    #endregion Resource Management
}
