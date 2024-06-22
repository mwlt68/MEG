using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nest;

namespace MEG.ElasticLogger.Base.Abstract;

public interface IAuditLogger
{
    Task<List<IndexResponse?>> AddAsync(ChangeTracker changeTracker,
        CancellationToken cancellationToken);
}