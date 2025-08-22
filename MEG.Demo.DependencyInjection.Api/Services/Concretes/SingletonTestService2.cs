using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class SingletonTestService2 : ISingletonTestService2
{
    public string GetSingletonMessage() => " Hello from SingletonTestService - 2!";
}
