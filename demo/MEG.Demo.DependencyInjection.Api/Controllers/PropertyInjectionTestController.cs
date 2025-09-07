using MEG.Demo.DependencyInjection.Api.Constants;
using MEG.Demo.DependencyInjection.Api.Services.Concretes;
using MEG.Demo.DependencyInjection.Api.Services.Interfaces;
using MEG.DependencyInjection.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.DependencyInjection.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertyInjectionTestController : ControllerBase
{
    public SingletonTestService SingletonService { get; set; }
    public ScopedTestService ScopedService { get; set; }
    public TransientTestService TransientService { get; set; }
    public ITransientTestService2 TransientTestService2 { get; set; }
    public IScopedTestService2 ScopedTestService2 { get; set; }
    public ISingletonTestService2 SingletonTestService2 { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedScoped2)] public IKeyedScopedTestService KeyedScopedTestService2 { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedSingleton2)] public IKeyedSingletonTestService KeyedSingletonTestService2 { get; set; }
    [AutoKeyed(ServiceKeyConstants.KeyedTransient2)] public IKeyedTransientTestService KeyedTransientTestService2 { get; set; }
    public PropertyInjectionTestService PropertyInjectionTestService { get; set; }


    [HttpGet("GetServices")]
    public IActionResult GetServices()
    {
        return Ok(
            new
            {
                SignletonMessage = SingletonService.GetSingletonMessage(),
                ScopedMessage = ScopedService.GetScopedMessage(),
                TransientMessage = TransientService.GetTransientMessage(),
            });
    }

    [HttpGet("GetServices-2")]
    public IActionResult GetServices2()
    {
        return Ok(
            new
            {
                SignletonTestServiceMessage = SingletonTestService2.GetSingletonMessage(),
                ScopedTestServiceMessage = ScopedTestService2.GetScopedMessage(),
                TransientTestServiceMessage = TransientTestService2.GetTransientMessage(),
            });
    }


    [HttpGet("GetKeyedServices-2")]
    public IActionResult GetServices3()
    {
        return Ok(
            new
            {
                KeyedSignletonTestServiceMessage = KeyedSingletonTestService2.GetKeyedSingletonMessage(),
                KeyedScopedTestServiceMessage = KeyedScopedTestService2.GetKeyedScopedMessage(),
                KeyedTransientTestServiceMessage = KeyedTransientTestService2.GetKeyedTransientMessage(),
            });
    }

    [HttpGet("GetServices-3")]
    public IActionResult GetServices4()
    {
        return Ok(
            PropertyInjectionTestService.GetServiceMessages());
    }

    [HttpGet("GetServices-4")]
    public IActionResult GetServices5()
    {
        return Ok(
            PropertyInjectionTestService.GetServiceMessages2());
    }

    [HttpGet("GetKeyedServices")]
    public IActionResult GetServices6()
    {
        return Ok(
            PropertyInjectionTestService.GetKeyedServiceMessages());
    }
}
