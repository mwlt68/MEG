using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Interfaces;

public interface IServiceRegistrarBase
{
    void Register(IServiceCollection services, Type serviceInterface, Type implementationType, object? serviceKey);
    void Register(IServiceCollection services, Type implementationType, object? serviceKey);
}

