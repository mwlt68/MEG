using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Extensions;
using MEG.ElasticLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Nest;

namespace MEG.ElasticLogger.Base.Concrete;

public class ExceptionLogger<TExceptionLoggerModel>(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ElasticLoggerBase(elasticClient,
        elasticLoggerSettings), IExceptionLogger
    where TExceptionLoggerModel : ExceptionLoggerModel, new()
{
    protected override LoggerType LoggerType => LoggerType.Exception;

    public async Task<IndexResponse?> AddAsync(HttpContext context, Exception exception, int statusCode,
        string? messageCode = null)
    {
        if (!IsLoggerTypeActive)
            return null;

        var elasticExceptionModel = GetElasticExceptionModel(exception, statusCode, messageCode);

        await elasticExceptionModel.SetContextRequestBaseModelAsync(context);

        var indexDocumentResult =
            await _elasticClient.IndexAsync(elasticExceptionModel,
                i => i.Index(_elasticLoggerSettings.ExceptionLoggerIndex));

        await elasticLogger.HandleIndexResponseAsync(indexDocumentResult);

        return indexDocumentResult;
    }

    protected virtual TExceptionLoggerModel GetElasticExceptionModel(Exception exception, int statusCode,
        string? messageCode)
    {
        return new TExceptionLoggerModel()
        {
            StatusCode = statusCode,
            MessageCode = messageCode,
            Message = exception.Message,
            InnerMessage = exception.InnerException?.Message,
            Source = exception.Source,
            StackTrace = exception.StackTrace,
            HResult = exception.HResult,
            OperatorId = elasticLogger.GetOperatorId()
        };
    }
}