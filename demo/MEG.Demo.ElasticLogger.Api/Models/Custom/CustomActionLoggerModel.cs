using MEG.ElasticLogger.Models;

namespace MEG.Demo.ElasticLogger.Api.Models.Custom;

public class CustomActionLoggerModel : ActionLoggerModel
{
    public string CustomProperty { get; set; }
}