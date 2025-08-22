using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedSingletonTestService : IKeyedSingletonTestService
{ public object ServiceKey => "KeyedSingletonService";
    public string GetKeyedSingletonMessage() => " Hello from KeyedSingletonTestService!";
}
