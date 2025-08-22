using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedSingletonTestService2 : IKeyedSingletonTestService
{ public object ServiceKey => "KeyedSingletonService-2";
    public string GetKeyedSingletonMessage() => " Hello from KeyedSingletonTestService-2!";
}
