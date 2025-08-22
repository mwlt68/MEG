using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedTransientTestService2 : IKeyedTransientTestService
{
    public string GetKeyedTransientMessage() => " Hello from KeyedTransientTestService-2!";
    public object ServiceKey => "KeyedTransientService-2";
}
