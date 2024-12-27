
namespace Synaptic.Core;

public partial class SynapticHub
{
    private class ResourceManager : IResourceService
    {
        private static Dictionary<Type, object> Resources { get; } = new Dictionary<Type, object>();

        internal ResourceManager() { }

        /// <summary>
        /// Get a resource from the resource container.
        /// </summary>
        /// <param name="resourceType">The type of the resource to get</param>
        /// <returns>The resource</returns>
        public object GetResource(Type resourceType)
        {
            if (!Resources.TryGetValue(resourceType, out var resource))
            {
                //  handle missing resource
            }

            return resource;
        }
        /// <summary>
        /// Get a resource from the resource manager.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource to get</typeparam>
        /// <returns>The resource</returns>
        public TResource GetResource<TResource>() where TResource : class
        {
            return GetResource(typeof(TResource)) as TResource;
        }
        /// <summary>
        /// Register a resource with the resource manager.
        /// </summary>
        /// <typeparam name="TResource">The type of the resource to register</typeparam>
        /// <param name="resource">The resource to register</param>
        /// <returns>The registered resource</returns>
        public TResource RegisterResource<TResource>(TResource resource) where TResource : class
        {
            if (resource == null)
            {
                //  handle null resource
            }

            return (Resources[typeof(TResource)] = resource) as TResource;
        }

        public void OnRegistered(IServiceContainer serviceContainer)
        {
            //  nothing to do here ..
        }
    }
}
