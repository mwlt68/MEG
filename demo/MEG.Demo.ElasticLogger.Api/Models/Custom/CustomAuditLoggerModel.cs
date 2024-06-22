using MEG.ElasticLogger.Models;

namespace MEG.Demo.ElasticLogger.Api.Models.Custom;

public class CustomAuditLoggerModel :AuditLoggerModel
{
    public string CustomProperty{ get; set; }
}