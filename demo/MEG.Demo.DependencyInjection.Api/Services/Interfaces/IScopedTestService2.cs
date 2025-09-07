using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Interfaces;

public interface IScopedTestService2 : IScopedService
{
    string GetScopedMessage();
}
