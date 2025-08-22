using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Interfaces;

public interface ISingletonTestService2 : ISingletonService
{
    string GetSingletonMessage();
}
