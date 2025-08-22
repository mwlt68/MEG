using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class SingletonServiceRegistrar : IServiceRegistrar < ISingletonService>
{
    public void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey) =>
        services.AddSingleton(serviceInterface, implementationType);

    public void Register(IServiceCollection services, Type implementationType,object? serviceKey) =>
        services.AddSingleton(implementationType);
}
