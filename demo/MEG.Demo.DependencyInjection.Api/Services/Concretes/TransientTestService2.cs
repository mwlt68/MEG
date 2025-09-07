using MEG.Demo.DependencyInjection.Api.Services.Interfaces;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class TransientTestService2 : ITransientTestService2
{
    public string GetTransientMessage() => " Hello from TransientTestService - 2!";
}
