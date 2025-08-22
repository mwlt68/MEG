using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class SingletonTestService : ISingletonService
{
    public string GetSingletonMessage() => " Hello from SingletonTestService!";
}
