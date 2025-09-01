using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public abstract class ServiceRegistrar(PropertyInjectionService propertyInjectionService, AddServiceOption options)
{
    protected abstract ServiceLifetime GetServiceLifetime();

    public void Register(IServiceCollection services, Type implementationType, Type? serviceInterface,
        object? serviceKey, bool isAutoInjectActive = false)
    {
        var serviceDescriptor =
            GetServiceDescriptor(implementationType, serviceInterface, serviceKey);
        services.Add(serviceDescriptor);
    }

    private ServiceDescriptor GetServiceDescriptor(Type implementationType, Type? serviceInterface, object? serviceKey)
    {
        if (serviceKey == null)
        {
            return serviceInterface != null
                ? options.IsAutoInjectActive
                    ? ServiceDescriptor.Describe(serviceInterface,
                        serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                    : ServiceDescriptor.Describe(serviceInterface, implementationType, GetServiceLifetime())
                : options.IsAutoInjectActive
                    ? ServiceDescriptor.Describe(implementationType,
                        serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                    : ServiceDescriptor.Describe(implementationType, implementationType, GetServiceLifetime());
        }

        {
            if (serviceInterface != null)
            {
                return options.IsAutoInjectActive
                    ? new ServiceDescriptor(serviceInterface, serviceKey,
                        (serviceProvider, key) => CreateInstance(serviceProvider, implementationType),
                        GetServiceLifetime())
                    : new ServiceDescriptor(serviceInterface, serviceKey, implementationType, GetServiceLifetime());
            }

            return options.IsAutoInjectActive
                ? new ServiceDescriptor(implementationType, serviceKey,
                    (serviceProvider, key) => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                : new ServiceDescriptor(implementationType, serviceKey, implementationType, GetServiceLifetime());
        }
    }

    private object CreateInstance(IServiceProvider serviceProvider, Type implementationType)
    {
        var instance = ActivatorUtilities.CreateInstance(serviceProvider, implementationType);
        propertyInjectionService.Inject(instance, serviceProvider);
        return instance;
    }
}
