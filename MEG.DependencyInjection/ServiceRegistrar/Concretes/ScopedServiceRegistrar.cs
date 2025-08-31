using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class ScopedServiceRegistrar : ServiceRegistrar, IServiceRegistrar<IScopedService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Scoped;
}
