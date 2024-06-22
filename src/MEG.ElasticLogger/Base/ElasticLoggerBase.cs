using MEG.ElasticLogger.Models;

using Microsoft.Extensions.Options;
using Nest;

namespace MEG.ElasticLogger.Base;

public abstract class ElasticLoggerBase(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings)
{
    protected readonly IElasticClient _elasticClient = elasticClient;
    protected readonly ElasticLoggerSettings _elasticLoggerSettings = elasticLoggerSettings.Value;
    protected abstract LoggerType LoggerType { get; }

    public bool IsLoggerTypeActive => IsActive();
    
    private bool IsActive()
    {
        if (!_elasticLoggerSettings.IsActive)
            return false;
        return LoggerType switch
        {
            LoggerType.Action => _elasticLoggerSettings.IsActionLoggerIndexActive,
            LoggerType.Audit => _elasticLoggerSettings.IsAuditLoggerActive,
            LoggerType.Exception => _elasticLoggerSettings.IsExceptionLoggerActive,
            _ => false
        };
    }
    
}