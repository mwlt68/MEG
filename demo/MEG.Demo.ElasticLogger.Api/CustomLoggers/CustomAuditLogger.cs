using MEG.Demo.ElasticLogger.Api.Models.Custom;
using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.Base.Concrete;
using MEG.ElasticLogger.ContractResolvers;
using MEG.ElasticLogger.Models;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;

namespace MEG.Demo.ElasticLogger.Api.CustomLoggers;

public class CustomAuditLogger(
    IElasticClient elasticClient,
    IHttpContextAccessor httpContextAccessor,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : AuditLogger<CustomAuditLoggerModel>(elasticClient, elasticLoggerSettings, elasticLogger)
{
    protected override CustomAuditLoggerModel GetAuditLoggerModel(object entity, string serializedEntity,
        string entityEntryState)
    {
        var model = base.GetAuditLoggerModel(entity, serializedEntity, entityEntryState);
        model.CustomProperty = "Custom Value";
        return model;
    }

    protected override JsonSerializerSettings GetJsonSerializerSettings() => new()
    {
        ContractResolver = new IgnoringVirtualPropertiesContractResolver()
    };
}