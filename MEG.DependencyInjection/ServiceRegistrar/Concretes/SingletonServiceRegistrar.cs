using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class SingletonServiceRegistrar :ServiceRegistrar, IServiceRegistrar <ISingletonService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Singleton;
}
