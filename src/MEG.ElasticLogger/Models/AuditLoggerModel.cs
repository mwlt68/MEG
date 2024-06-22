namespace MEG.ElasticLogger.Models;

public class AuditLoggerModel
{
    public string? EntityId { get; set; }
    public string? EntityJson { get; set; }
    public DateTime OperationDate { get; set; }=DateTime.UtcNow;
    public string? EntityOperationType { get; set; }
    public string? OperatorId { get; set; }
    public string? EntityTypeName { get; set; }
}

public enum EntityOperationType
{
    Added,
    Modified,
    Deleted
}