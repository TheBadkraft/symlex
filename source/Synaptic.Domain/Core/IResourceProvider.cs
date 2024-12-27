namespace Synaptic.Core;

#nullable enable
public interface IResourceProvider
{
    /// <summary>
    /// Get a resource from the resource container.
    /// </summary>
    /// <param name="resourceType">The type of the resource to get</param>
    /// <returns>The resource</returns>
    object? GetResource(Type resourceType);
}