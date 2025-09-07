using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedScopedTestService : IKeyedScopedTestService
{
    public object ServiceKey => "KeyedScopedService";
    public string GetKeyedScopedMessage() => " Hello from KeyedScopedTestService!";
}
