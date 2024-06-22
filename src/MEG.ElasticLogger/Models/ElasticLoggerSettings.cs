namespace MEG.ElasticLogger.Models;

public class ElasticLoggerSettings
{
    public bool IsActive { get; set; } = true;
    public string? ElasticSearchUrl { get; set; }
    public bool IsAuditLoggerActive { get; set; } = true;
    public bool IsExceptionLoggerActive { get; set; } = true;
    public bool IsActionLoggerIndexActive { get; set; } = true;
    public string AuditLoggerIndex { get; set; } = "audit-logger";
    public string ExceptionLoggerIndex { get; set; } = "exception-logger";
    public string ActionLoggerIndex { get; set; } = "action-logger";
    public string? DefaultIndex { get; set; }
}