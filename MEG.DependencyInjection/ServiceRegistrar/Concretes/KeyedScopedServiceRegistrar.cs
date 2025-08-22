using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class KeyedScopedServiceRegistrar : IServiceRegistrar< IKeyedScopedService>
{

    public void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey) =>
        services.AddKeyedScoped(serviceInterface,serviceKey,implementationType);

    public void Register(IServiceCollection services, Type implementationType,object? serviceKey) =>
        services.AddKeyedScoped(implementationType,serviceKey);
}
