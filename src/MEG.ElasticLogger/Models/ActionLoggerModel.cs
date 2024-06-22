namespace MEG.ElasticLogger.Models;
public class ActionLoggerModel : ContextRequestBaseModel
{
    public Guid? ActionId { get; set; }
    public string? ResponseBodyJson { get; set; }
    public int? ResponseStatusCode { get; set; }
    public new ActionLoggerType ActionLoggerType { get; set; }
}

public enum ActionLoggerType
{
    Executing,
    Executed
}