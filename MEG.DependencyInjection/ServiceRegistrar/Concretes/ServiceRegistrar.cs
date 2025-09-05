using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public abstract class ServiceRegistrar(PropertyInjectionService propertyInjectionService, AddServiceOption options)
{
    protected abstract ServiceLifetime GetServiceLifetime();

    public void Register(IServiceCollection services, Type implementationType, Type? serviceInterface,
        object? serviceKey)
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
                ? ServiceDescriptor.Describe(serviceInterface,
                    serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime())
                : ServiceDescriptor.Describe(implementationType,
                    serviceProvider => CreateInstance(serviceProvider, implementationType), GetServiceLifetime());
        }

        if (serviceInterface != null)
        {
            return new ServiceDescriptor(serviceInterface, serviceKey,
                (serviceProvider, key) => CreateInstance(serviceProvider, implementationType),
                GetServiceLifetime());
        }

        return new ServiceDescriptor(implementationType, serviceKey,
            (serviceProvider, key) => CreateInstance(serviceProvider, implementationType), GetServiceLifetime());
    }

    private object CreateInstance(IServiceProvider serviceProvider, Type implementationType)
    {
        var instance = ActivatorUtilities.CreateInstance(serviceProvider, implementationType);
        if (options.IsAutoInjectActive)
            propertyInjectionService.Inject(instance, serviceProvider);
        return instance;
    }
}
