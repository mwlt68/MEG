using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedScopedTestService2 : IKeyedScopedTestService
{
    public object ServiceKey => "KeyedScopedService-2";
    public string GetKeyedScopedMessage() => " Hello from KeyedScopedTestService-2!";
}
