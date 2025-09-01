using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class ScopedServiceRegistrar(PropertyInjectionService propertyInjectionService, AddServiceOption options) :
    ServiceRegistrar(propertyInjectionService, options), IServiceRegistrar<IScopedService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Scoped;
}
