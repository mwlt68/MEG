using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Interfaces;

public interface IServiceRegistrarBase
{
    public void Register(IServiceCollection services, Type implementationType, Type? serviceInterface,
        object? serviceKey, bool isAutoInjectActive = false);
}
