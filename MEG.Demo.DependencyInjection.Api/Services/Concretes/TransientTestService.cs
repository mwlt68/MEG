using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class TransientTestService : ITransientService
{
    public string GetTransientMessage() => " Hello from TransientTestService!";
}
