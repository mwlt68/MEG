using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class ScopedTestService2 : IScopedTestService2
{
    public string GetScopedMessage() => " Hello from ScopedTestService - 2!";
}
