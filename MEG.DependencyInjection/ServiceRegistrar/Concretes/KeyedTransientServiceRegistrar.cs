using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class KeyedTransientServiceRegistrar : IServiceRegistrar< IKeyedTransientService>
{
    public void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey) =>
        services.AddKeyedTransient(serviceInterface,serviceKey,implementationType);

    public void Register(IServiceCollection services, Type implementationType,object? serviceKey) =>
        services.AddKeyedTransient(implementationType,serviceKey);
}
