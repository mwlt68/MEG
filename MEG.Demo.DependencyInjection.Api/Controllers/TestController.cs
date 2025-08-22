using MEG.Demo.DependencyInjection.Api.Constants;
using MEG.Demo.DependencyInjection.Api.Services.Concretes;
using MEG.Demo.DependencyInjection.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MEG.Demo.DependencyInjection.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController()
    : ControllerBase
{
    [HttpGet("1")]
    public IActionResult GetServices([FromServices] SingletonTestService singletonTestService,
        [FromServices] ScopedTestService scopedTestService, [FromServices] TransientTestService transientTestService)
    {
        return Ok(
            new
            {
                SignletonMessage = singletonTestService.GetSingletonMessage(),
                ScopedMessage = scopedTestService.GetScopedMessage(),
                TransientMessage = transientTestService.GetTransientMessage(),
            });
    }

    [HttpGet("2")]
    public IActionResult GetServices2([FromServices] ISingletonTestService2 singletonTestService2,
        [FromServices] IScopedTestService2 scopedTestService2,
        [FromServices] ITransientTestService2 transientTestService2)
    {
        return Ok(
            new
            {
                SignletonTestServiceMessage = singletonTestService2.GetSingletonMessage(),
                ScopedTestServiceMessage = scopedTestService2.GetScopedMessage(),
                TransientTestServiceMessage = transientTestService2.GetTransientMessage(),
            });
    }


    [HttpGet("3")]
    public IActionResult GetServices3(
        [FromKeyedServices(ServiceKeyConstants.KeyedSingleton)]
        IKeyedSingletonTestService keyedSingletonTestService,
        [FromKeyedServices(ServiceKeyConstants.KeyedScoped)]
        IKeyedScopedTestService keyedScopedTestService,
        [FromKeyedServices(ServiceKeyConstants.KeyedTransient)]
        IKeyedTransientTestService keyedTransientTestService)
    {
        return Ok(
            new
            {
                KeyedSignletonTestServiceMessage = keyedSingletonTestService.GetKeyedSingletonMessage(),
                KeyedScopedTestServiceMessage = keyedScopedTestService.GetKeyedScopedMessage(),
                KeyedTransientTestServiceMessage = keyedTransientTestService.GetKeyedTransientMessage(),
            });
    }

    [HttpGet("4")]
    public IActionResult GetServices4(
        [FromKeyedServices(ServiceKeyConstants.KeyedSingleton2)]
        IKeyedSingletonTestService keyedSingletonTestService,
        [FromKeyedServices(ServiceKeyConstants.KeyedScoped2)]
        IKeyedScopedTestService keyedScopedTestService,
        [FromKeyedServices(ServiceKeyConstants.KeyedTransient2)]
        IKeyedTransientTestService keyedTransientTestService)
    {
        return Ok(
            new
            {
                KeyedSignletonTestServiceMessage = keyedSingletonTestService.GetKeyedSingletonMessage(),
                KeyedScopedTestServiceMessage = keyedScopedTestService.GetKeyedScopedMessage(),
                KeyedTransientTestServiceMessage = keyedTransientTestService.GetKeyedTransientMessage(),
            });
    }
}
