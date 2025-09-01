using MEG.DependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace MEG.DependencyInjection.Activators;

public class PropertyInjectingControllerActivator(PropertyInjectionService propertyInjectionService) : IControllerActivator
{
    public object Create(ControllerContext context)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();
        var controller = ActivatorUtilities.CreateInstance(serviceProvider, controllerType);
        propertyInjectionService.Inject(controller, serviceProvider);
        return controller;
    }

    public void Release(ControllerContext context, object controller) { }
}
