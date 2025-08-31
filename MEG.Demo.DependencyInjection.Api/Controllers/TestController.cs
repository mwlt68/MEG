using MEG.Demo.DependencyInjection.Api.Constants;
using MEG.Demo.DependencyInjection.Api.Services.Concretes;
using MEG.Demo.DependencyInjection.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.DependencyInjection.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(
    SingletonTestService singletonTestService,
    ScopedTestService scopedTestService,
    TransientTestService transientTestService,
    ISingletonTestService2 singletonTestService2,
    IScopedTestService2 scopedTestService2,
    ITransientTestService2 transientTestService2,
    [FromKeyedServices(ServiceKeyConstants.KeyedSingleton)]
    IKeyedSingletonTestService keyedSingletonTestService,
    [FromKeyedServices(ServiceKeyConstants.KeyedScoped)]
    IKeyedScopedTestService keyedScopedTestService,
    [FromKeyedServices(ServiceKeyConstants.KeyedTransient)]
    IKeyedTransientTestService keyedTransientTestService,
    [FromKeyedServices(ServiceKeyConstants.KeyedSingleton2)]
    IKeyedSingletonTestService keyedSingletonTestService2,
    [FromKeyedServices(ServiceKeyConstants.KeyedScoped2)]
    IKeyedScopedTestService keyedScopedTestService2,
    [FromKeyedServices(ServiceKeyConstants.KeyedTransient2)]
    IKeyedTransientTestService keyedTransientTestService2)
    : ControllerBase
{
    private readonly SingletonTestService _singletonTestService = singletonTestService;
    private readonly ScopedTestService _scopedTestService = scopedTestService;
    private readonly TransientTestService _transientTestService = transientTestService;
    private readonly ISingletonTestService2 _singletonTestService2 = singletonTestService2;
    private readonly IScopedTestService2 _scopedTestService2 = scopedTestService2;
    private readonly ITransientTestService2 _transientTestService2 = transientTestService2;


    private readonly IKeyedSingletonTestService _keyedSingletonTestService = keyedSingletonTestService;

    private readonly IKeyedScopedTestService _keyedScopedTestService = keyedScopedTestService;

    private readonly IKeyedTransientTestService _keyedTransientTestService = keyedTransientTestService;

    private readonly IKeyedSingletonTestService _keyedSingletonTestService2 = keyedSingletonTestService2;

    private readonly IKeyedScopedTestService _keyedScopedTestService2 = keyedScopedTestService2;

    private readonly IKeyedTransientTestService _keyedTransientTestService2 = keyedTransientTestService2;


    [HttpGet("GetServices")]
    public IActionResult GetServices()
    {
        return Ok(
            new
            {
                SignletonMessage = _singletonTestService.GetSingletonMessage(),
                ScopedMessage = _scopedTestService.GetScopedMessage(),
                TransientMessage = _transientTestService.GetTransientMessage(),
            });
    }

    [HttpGet("GetServices-2")]
    public IActionResult GetServices2()
    {
        return Ok(
            new
            {
                SignletonTestServiceMessage = _singletonTestService2.GetSingletonMessage(),
                ScopedTestServiceMessage = _scopedTestService2.GetScopedMessage(),
                TransientTestServiceMessage = _transientTestService2.GetTransientMessage(),
            });
    }


    [HttpGet("GetKeyedServices")]
    public IActionResult GetServices3()
    {
        return Ok(
            new
            {
                KeyedSignletonTestServiceMessage = _keyedSingletonTestService.GetKeyedSingletonMessage(),
                KeyedScopedTestServiceMessage = _keyedScopedTestService.GetKeyedScopedMessage(),
                KeyedTransientTestServiceMessage = _keyedTransientTestService.GetKeyedTransientMessage(),
            });
    }

    [HttpGet("GetKeyedServices-2")]
    public IActionResult GetServices4()
    {
        return Ok(
            new
            {
                KeyedSignletonTestServiceMessage = _keyedSingletonTestService2.GetKeyedSingletonMessage(),
                KeyedScopedTestServiceMessage = _keyedScopedTestService2.GetKeyedScopedMessage(),
                KeyedTransientTestServiceMessage = _keyedTransientTestService2.GetKeyedTransientMessage(),
            });
    }
}
