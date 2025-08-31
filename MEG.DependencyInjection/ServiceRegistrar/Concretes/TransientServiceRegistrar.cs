using MEG.DependencyInjection.ServiceRegistrar.Interfaces;
using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Concretes;

public class TransientServiceRegistrar : ServiceRegistrar, IServiceRegistrar<ITransientService>
{
    protected override ServiceLifetime GetServiceLifetime() => ServiceLifetime.Transient;
}
