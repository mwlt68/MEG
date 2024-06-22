using MEG.ElasticLogger.Models;

namespace MEG.Demo.ElasticLogger.Api.Models.Custom;

public class CustomExceptionLoggerModel : ExceptionLoggerModel
{
    public string? CustomProperty { get; set; }
}