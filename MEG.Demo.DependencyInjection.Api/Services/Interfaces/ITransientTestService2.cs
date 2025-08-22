using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Interfaces;

public interface ITransientTestService2 : ITransientService
{
    string GetTransientMessage();
}
