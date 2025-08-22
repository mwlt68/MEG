namespace MEG.DependencyInjection.Services;

public interface IKeyedBaseService : IBaseService
{
    public object? ServiceKey { get;  }
}
