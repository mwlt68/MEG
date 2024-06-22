using MEG.ElasticLogger.Base.Abstract;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MEG.ElasticLogger.Filters;

public class ActionLoggerFilter(IActionLogger actionLogger) : IActionFilter
{
    public async void OnActionExecuting(ActionExecutingContext actionExecutingContext)
    {
        await actionLogger.OnActionExecutingAsync(actionExecutingContext);
    }

    public async void OnActionExecuted(ActionExecutedContext actionExecutingContext)
    {
        await actionLogger.OnActionExecutedAsync(actionExecutingContext);
    }
}