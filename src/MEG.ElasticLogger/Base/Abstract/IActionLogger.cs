using Microsoft.AspNetCore.Mvc.Filters;

namespace MEG.ElasticLogger.Base.Abstract;

public interface IActionLogger
{
    Task OnActionExecutingAsync(ActionExecutingContext actionExecutingContext);
    Task OnActionExecutedAsync(ActionExecutedContext actionExecutingContext);
}