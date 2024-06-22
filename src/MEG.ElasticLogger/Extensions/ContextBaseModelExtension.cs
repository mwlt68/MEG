using MEG.ElasticLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MEG.ElasticLogger.Extensions;

public static class ContextBaseModelExtension
{
    public static void SetContextBaseModel<TModel>(this TModel model,HttpContext context) where TModel : ContextBaseModel
    {
        model.RequestPath = context.Request.Path;
        model.RequestRouteController = context.GetRouteValue("controller")?.ToString();
        model.RequestRouteAction = context.GetRouteValue("action")?.ToString();
        model.RequestMethod = context.Request.Method;
        model.RequestHost = context.Request.Host.Value;
    }
    
    public static async Task SetContextRequestBaseModelAsync<TModel>(this TModel model,HttpContext context) where TModel : ContextRequestBaseModel
    {
        model.SetContextBaseModel(context);
        model.RequestHeaderDicStr = context.Request.Headers.ToDictionary().DictionaryToString();
        model.RequestQuery = context.Request.QueryString.Value;
        model.RequestBodyJson = await context.Request.GetRequestBodyAsync();
        model.RequestCookieDicStr = context.Request.Cookies.ToDictionary().DictionaryToString();
        model.RequestFormDicStr = context.Request.HasFormContentType
            ? context.Request.Form.ToDictionary().DictionaryToString()
            : null;
    }
}