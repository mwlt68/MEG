using MEG.DependencyInjection.Models;
using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class TransientServiceRegistrar(PropertyInjectionService propertyInjectionService, AddServiceOption options) :
    ServiceRegistrar(propertyInjectionService, options), IServiceRegistrar<ITransientService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Transient;
}
