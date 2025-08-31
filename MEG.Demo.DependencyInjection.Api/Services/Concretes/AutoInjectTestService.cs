using MEG.Demo.DependencyInjection.Api.Constants;
using MEG.Demo.DependencyInjection.Api.Services.Interfaces;
using MEG.DependencyInjection.Attributes;
using MEG.DependencyInjection.Services;

namespace MEG.Demo.DependencyInjection.Api.Services.Concretes;

public class AutoInjectTestService : ITransientService
{
    public SingletonTestService SingletonService { get; set; }
    public ScopedTestService ScopedService { get; set; }
    public TransientTestService TransientService { get; set; }
    public ITransientTestService2 TransientTestService2 { get; set; }
    public IScopedTestService2 ScopedTestService2 { get; set; }
    public ISingletonTestService2 SingletonTestService2 { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedScoped)] public IKeyedScopedTestService KeyedScopedTestService { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedSingleton)] public IKeyedSingletonTestService KeyedSingletonTestService { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedTransient)] public IKeyedTransientTestService KeyedTransientTestService { get; set; }
    public ILogger<AutoInjectTestService> Logger { get; set; }
    public IHostEnvironment HostEnvironment { get; set; }
    public IHttpContextAccessor HttpContextAccessor { get; set; }

    private readonly string _instanceId;

    public AutoInjectTestService() =>
        _instanceId = Guid.NewGuid().ToString();

    public object GetServiceMessages()
    {
        var logMessage =
            $"Environment : {HostEnvironment.EnvironmentName}, Method : {HttpContextAccessor.HttpContext?.Request.Method} , Function : GetServiceMessages";

        Logger?.LogInformation(logMessage);
        return new
        {
            InstanceId = _instanceId,
            SingletonService = SingletonService?.GetSingletonMessage(),
            ScopedService = ScopedService?.GetScopedMessage(),
            TransientTestService = TransientService?.GetTransientMessage(),
        };
    }

    public object GetServiceMessages2()
    {
        var logMessage =
            $"Environment : {HostEnvironment.EnvironmentName}, Method : {HttpContextAccessor.HttpContext?.Request.Method} , Function : GetServiceMessages2";
        Logger?.LogInformation(logMessage);

        return new
        {
            InstanceId = _instanceId,
            TransientTestService2 = TransientTestService2?.GetTransientMessage(),
            ScopedTestService2 = ScopedTestService2?.GetScopedMessage(),
            SingletonTestService2 = SingletonTestService2?.GetSingletonMessage(),
        };
    }

    public object GetKeyedServiceMessages()
    {
        var logMessage =
            $"Environment : {HostEnvironment.EnvironmentName}, Method : {HttpContextAccessor.HttpContext?.Request.Method} , Function : GetKeyedServiceMessages";

        Logger?.LogInformation(logMessage);

        return new
        {
            InstanceId = _instanceId,
            KeyedScopedTestService = KeyedScopedTestService.GetKeyedScopedMessage(),
            KeyedSingletonTestService = KeyedSingletonTestService.GetKeyedSingletonMessage(),
            KeyedTransientTestService = KeyedTransientTestService.GetKeyedTransientMessage(),
        };
    }
}
