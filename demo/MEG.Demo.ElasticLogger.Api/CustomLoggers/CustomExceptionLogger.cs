using MEG.Demo.ElasticLogger.Api.Models.Custom;
using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Base.Concrete;
using MEG.ElasticLogger.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace MEG.Demo.ElasticLogger.Api.CustomLoggers;

public class CustomExceptionLogger(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger) : ExceptionLogger<CustomExceptionLoggerModel>(elasticClient,
    elasticLoggerSettings, elasticLogger)
{
    protected override CustomExceptionLoggerModel GetElasticExceptionModel(Exception exception, int statusCode,
        string? messageCode)
    {
        var model = base.GetElasticExceptionModel(exception, statusCode, messageCode);
        model.CustomProperty = "Elastic Exception Model Value";
        return model;
    }
}