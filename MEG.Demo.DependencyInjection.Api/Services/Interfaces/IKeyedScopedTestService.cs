using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Interfaces;

public interface IKeyedScopedTestService : IKeyedScopedService
{
    string GetKeyedScopedMessage();
}
