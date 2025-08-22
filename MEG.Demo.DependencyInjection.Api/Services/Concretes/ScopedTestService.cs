using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class ScopedTestService : IScopedService
{
    public string GetScopedMessage() => " Hello from ScopedTestService!";
}
