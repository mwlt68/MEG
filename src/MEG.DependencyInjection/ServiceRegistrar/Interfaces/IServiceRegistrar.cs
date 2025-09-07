using MEG.DependencyInjection.Services;

namespace MEG.DependencyInjection.ServiceRegistrar.Interfaces;

public interface IServiceRegistrar<TService> : IServiceRegistrarBase where  TService : IBaseService
{
}
