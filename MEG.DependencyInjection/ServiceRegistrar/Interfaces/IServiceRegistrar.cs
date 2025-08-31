using MEG.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.ServiceRegistrar.Interfaces;

public interface IServiceRegistrar<TService> : IServiceRegistrarBase where  TService : IBaseService
{
}
