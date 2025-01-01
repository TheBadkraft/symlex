
using Synaptic.Services.Analysis;

namespace Synaptic.Core;

public partial class SynapticHub
{
    private class ServiceContainer : IServiceContainer
    {
        private static Dictionary<Type, ISynapticService> Services { get; } = new();

        internal ServiceContainer() { }

        /// <summary>
        /// Get a service from the service container.
        /// </summary>
        /// <param name="serviceType">The type of the service to get</param>
        /// <returns>The service</returns>
        public object GetService(Type serviceType)
        {
            return LocateService(serviceType);
        }
        /// <summary>
        /// Get a service from the service container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to get</typeparam>
        /// <returns>The service</returns>
        public TService GetService<TService>() where TService : class, ISynapticService
        {
            return GetService(typeof(TService)) as TService;
        }
        /// <summary>
        /// Register a service with the service container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to register</typeparam>
        /// <param name="service">The service to register</param>
        public TService RegisterService<TService>(TService service) where TService : class, ISynapticService
        {
            if (service == null)
            {
                //  handle null service
            }

            ((Services[typeof(TService)] = service) as TService).OnRegistered(this);
            return service;
        }

        /// <summary>
        /// [INTERNAL] Start a service.
        /// </summary>
        /// <typeparam name="TService">The type of the service to start</typeparam>
        internal void StartService<TService>() where TService : class, ISynapticService
        {
            LocateService(typeof(TService)).Start();
        }

        //  Locate services
        private static ISynapticService LocateService(Type serviceType)
        {
            if (!Services.TryGetValue(serviceType, out var service))
            {
                //  TODO: replace with error reporting and recover
                throw new ArgumentException($"Service ({serviceType.Name}) not found.");
            }

            return service;
        }
    }
}
