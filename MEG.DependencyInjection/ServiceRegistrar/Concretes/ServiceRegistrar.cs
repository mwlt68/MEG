using MEG.DependencyInjection.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public abstract class ServiceRegistrar
{
    protected abstract ServiceLifetime GetServiceLifetime();

    public void Register(IServiceCollection services, Type implementationType, Type? serviceInterface,
        object? serviceKey, bool isAutoInjectActive = false)
    {
        var serviceDescriptor =
            GetServiceDescriptor(implementationType, serviceInterface, serviceKey, isAutoInjectActive);
        services.Add(serviceDescriptor);
    }

    private ServiceDescriptor GetServiceDescriptor( Type implementationType,Type? serviceInterface, object? serviceKey,
        bool isAutoInjectActive)
    {
        if (serviceKey == null)
        {
            return serviceInterface != null
                ? isAutoInjectActive
                    ? ServiceDescriptor.Describe(serviceInterface,
                        serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                    : ServiceDescriptor.Describe(serviceInterface,implementationType, GetServiceLifetime())
                : isAutoInjectActive
                    ? ServiceDescriptor.Describe(implementationType,
                        serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                    : ServiceDescriptor.Describe(implementationType, implementationType, GetServiceLifetime());
        }

        {
            if (serviceInterface != null)
            {
                return isAutoInjectActive
                    ? new ServiceDescriptor(serviceInterface, serviceKey,
                        (serviceProvider, key) => CreateInstance(serviceProvider, implementationType),
                        GetServiceLifetime())
                    : new ServiceDescriptor(serviceInterface,serviceKey,implementationType, GetServiceLifetime());
            }

            return isAutoInjectActive
                ? new ServiceDescriptor(implementationType, serviceKey,
                    (serviceProvider, key) => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                : new ServiceDescriptor(implementationType, serviceKey, implementationType, GetServiceLifetime());
        }
    }

    private static object CreateInstance(IServiceProvider serviceProvider, Type implementationType)
    {
        var instance = ActivatorUtilities.CreateInstance(serviceProvider, implementationType);
        PropertyInjectionHelper.Inject(instance, serviceProvider);
        return instance;
    }
}
