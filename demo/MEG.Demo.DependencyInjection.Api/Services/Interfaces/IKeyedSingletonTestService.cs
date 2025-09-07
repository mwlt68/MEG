using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Interfaces;

public interface IKeyedSingletonTestService : IKeyedSingletonService
{
    string GetKeyedSingletonMessage();
}
