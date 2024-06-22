namespace MEG.ElasticLogger.Models;

public class ExceptionLoggerModel : ContextRequestBaseModel
{
    public int StatusCode { get; set; }
    public string? MessageCode { get; set; }
    public string? Message { get; set; }
    public string? InnerMessage { get; set; }
    public string? Source { get; set; }
    public string? StackTrace { get; set; }
    public int HResult { get; set; }
}