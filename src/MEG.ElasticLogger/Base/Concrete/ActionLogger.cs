using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Extensions;
using MEG.ElasticLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;

namespace MEG.ElasticLogger.Base.Concrete;

public class ActionLogger<TActionLoggerModel>(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ElasticLoggerBase(elasticClient, elasticLoggerSettings), IActionLogger
    where TActionLoggerModel : ActionLoggerModel, new()
{
    protected override LoggerType LoggerType => LoggerType.Action;
    private readonly Guid _guid = Guid.NewGuid();

    public async Task OnActionExecutingAsync(ActionExecutingContext actionExecutingContext)
    {
        if (!IsLoggerTypeActive)
            return;

        var actionExecutingLoggerModel = GetActionExecutingLoggerModel();

        await actionExecutingLoggerModel.SetContextRequestBaseModelAsync(actionExecutingContext.HttpContext);

        var indexDocumentResult =
            await _elasticClient.IndexAsync(actionExecutingLoggerModel,
                i => i.Index(_elasticLoggerSettings.ActionLoggerIndex));

        await elasticLogger.HandleIndexResponseAsync(indexDocumentResult);
    }

    protected virtual TActionLoggerModel GetActionExecutingLoggerModel()
    {
        return new TActionLoggerModel()
        {
            ActionLoggerType = ActionLoggerType.Executing,
            OperatorId = elasticLogger.GetOperatorId(),
            ActionId = _guid,
        };
    }


    public async Task OnActionExecutedAsync(ActionExecutedContext actionExecutingContext)
    {
        if (!IsLoggerTypeActive)
            return;

        ObjectResult? objectResult = null;
        if (actionExecutingContext.Result is ObjectResult objRes)
            objectResult = objRes;

        var actionExecutedLoggerModel = GetActionExecutedLoggerModel(objectResult);

        actionExecutedLoggerModel.SetContextBaseModel(actionExecutingContext.HttpContext);

        var indexDocumentResult =
            await _elasticClient.IndexAsync(actionExecutedLoggerModel,
                i => i.Index(_elasticLoggerSettings.ActionLoggerIndex));

        await elasticLogger.HandleIndexResponseAsync(indexDocumentResult);
    }

    protected virtual TActionLoggerModel GetActionExecutedLoggerModel(ObjectResult? objectResult)
    {
        return new TActionLoggerModel()
        {
            ResponseBodyJson = JsonConvert.SerializeObject(objectResult?.Value),
            ActionLoggerType = ActionLoggerType.Executed,
            OperatorId = elasticLogger.GetOperatorId(),
            ResponseStatusCode = objectResult?.StatusCode,
            ActionId = _guid
        };
    }
}