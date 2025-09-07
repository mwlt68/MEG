namespace MEG.DependencyInjection.Services;

public interface IKeyedService : IBaseService
{
    public object? ServiceKey { get;  }
}
