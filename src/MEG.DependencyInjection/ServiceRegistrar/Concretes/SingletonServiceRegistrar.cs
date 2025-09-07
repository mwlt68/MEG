using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class SingletonServiceRegistrar(PropertyInjectionService propertyInjectionService, AddServiceOption options) :
    ServiceRegistrar(propertyInjectionService, options), IServiceRegistrar<ISingletonService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Singleton;
}
