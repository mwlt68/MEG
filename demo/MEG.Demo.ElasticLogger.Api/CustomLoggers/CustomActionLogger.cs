using MEG.Demo.ElasticLogger.Api.Models;
using MEG.Demo.ElasticLogger.Api.Models.Custom;
using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Base.Concrete;
using MEG.ElasticLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;

namespace MEG.Demo.ElasticLogger.Api.CustomLoggers;

public class CustomActionLogger(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ActionLogger<CustomActionLoggerModel>(elasticClient, elasticLoggerSettings, elasticLogger)
{
    protected override CustomActionLoggerModel GetActionExecutedLoggerModel(ObjectResult? objectResult)
    {
        var model = base.GetActionExecutedLoggerModel(objectResult);
        model.CustomProperty = "Action Executed Logger Model Value";
        return model;
    }

    protected override CustomActionLoggerModel GetActionExecutingLoggerModel()
    {
        var model = base.GetActionExecutingLoggerModel();
        model.CustomProperty = "Action Executing Logger Model Value";
        return model;
    }
}