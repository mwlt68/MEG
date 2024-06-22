using MEG.ElasticLogger.Base.Abstract;
using MEG.ElasticLogger.ContractResolvers;
using MEG.ElasticLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;

namespace MEG.ElasticLogger.Base.Concrete;

public class AuditLogger<TAuditLoggerModel>(
    IElasticClient elasticClient,
    IOptions<ElasticLoggerSettings> elasticLoggerSettings,
    IElasticLogger elasticLogger)
    : ElasticLoggerBase(elasticClient,
        elasticLoggerSettings), IAuditLogger where TAuditLoggerModel : AuditLoggerModel, new()
{
    protected override LoggerType LoggerType => LoggerType.Audit;

    public async Task<List<IndexResponse?>> AddAsync(ChangeTracker changeTracker,
        CancellationToken cancellationToken)
    {
        if (!IsLoggerTypeActive)
            return new List<IndexResponse?>();

        var entryStates = GetEntityOperationTypes().Select(GetEntityState);

        var entityEntries = changeTracker
            .Entries()
            .Where(x => entryStates.Contains(x.State))
            .ToList();
        var indexResponses = new List<IndexResponse?>();
        var settings = GetJsonSerializerSettings();

        foreach (var entityEntry in entityEntries)
        {
            var entity = entityEntry.Entity;
            if (entity is IdenticalEntity)
            {
                var serializedEntity = JsonConvert.SerializeObject(entity, settings);
                var state = entityEntry.State.ToString();
                var entityAudit = GetAuditLoggerModel((entity as IdenticalEntity), serializedEntity, state);
                var indexDocumentResult = await _elasticClient.IndexAsync(entityAudit,
                    x => x.Index(_elasticLoggerSettings.AuditLoggerIndex), cancellationToken);
                indexResponses.Add(indexDocumentResult);
                await elasticLogger.HandleIndexResponseAsync(indexDocumentResult);
            }
        }

        return indexResponses;
    }

    protected virtual TAuditLoggerModel GetAuditLoggerModel(IdenticalEntity? entity, string serializedEntity,
        string entityEntryState)
    {
        return new TAuditLoggerModel()
        {
            EntityId = entity?.Id.ToString(),
            OperatorId = elasticLogger.GetOperatorId(),
            OperationDate = DateTime.UtcNow,
            EntityJson = serializedEntity,
            EntityOperationType = entityEntryState,
            EntityTypeName = entity?.GetType().Name
        };
    }

    protected virtual JsonSerializerSettings GetJsonSerializerSettings() => new()
    {
        ContractResolver = new IgnoringVirtualPropertiesContractResolver()
    };

    protected virtual List<EntityOperationType> GetEntityOperationTypes() =>
        Enum.GetValues(typeof(EntityOperationType)).Cast<EntityOperationType>().ToList();

    private EntityState GetEntityState(EntityOperationType entityOperationType)
    {
        return entityOperationType switch
        {
            EntityOperationType.Added => EntityState.Added,
            EntityOperationType.Deleted => EntityState.Deleted,
            EntityOperationType.Modified => EntityState.Modified,
            _ => throw new ArgumentException()
        };
    }
}