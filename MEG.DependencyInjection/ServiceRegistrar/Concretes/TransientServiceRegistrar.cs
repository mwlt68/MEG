using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class TransientServiceRegistrar : IServiceRegistrar<ITransientService>
{
    public void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey) =>
        services.AddTransient(serviceInterface, implementationType);

    public void Register(IServiceCollection services, Type implementationType,object? serviceKey) =>
        services.AddTransient(implementationType);
}
