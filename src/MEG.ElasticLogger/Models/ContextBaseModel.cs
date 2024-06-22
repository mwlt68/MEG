namespace MEG.ElasticLogger.Models;

public abstract class ContextBaseModel
{
    public DateTime OperationDate { get; set; } = DateTime.UtcNow;
    public string? OperatorId { get; set; }
    public string? RequestPath { get; set; }
    public string? RequestRouteController { get; set; }
    public string? RequestRouteAction { get; set; }
    public string? RequestMethod { get; set; }
    public string? RequestHost { get; set; }
}

public abstract class ContextRequestBaseModel : ContextBaseModel
{
    public string? RequestHeaderDicStr { get; set; }
    public string? RequestQuery { get; set; }
    public string? RequestBodyJson { get; set; }
    public string? RequestCookieDicStr { get; set; }
    public string? RequestFormDicStr { get; set; }
}