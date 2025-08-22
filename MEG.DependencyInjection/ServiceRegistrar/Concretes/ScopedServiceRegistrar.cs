using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class ScopedServiceRegistrar : IServiceRegistrar<IScopedService>
{
    public void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey) =>
        services.AddScoped(serviceInterface, implementationType);

    public void Register(IServiceCollection services, Type implementationType,object? serviceKey) =>
        services.AddScoped(implementationType);
}
