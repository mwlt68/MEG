using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Interfaces;

public interface IServiceRegistrar<TService> : IServiceRegistrarBase where  TService : IBaseService
{
    void Register(IServiceCollection services, Type serviceInterface, Type implementationType,object? serviceKey);
    void Register(IServiceCollection services,  Type implementationType,object? serviceKey);
}
