using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class KeyedTransientTestService : IKeyedTransientTestService
{
    public string GetKeyedTransientMessage() => " Hello from KeyedTransientTestService!";
    public object ServiceKey => "KeyedTransientService";
}
