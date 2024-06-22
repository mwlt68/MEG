using MEG.Demo.ElasticLogger.Api.DataAccess.Entities;
using MEG.ElasticLogger.Base.Abstract;
using Microsoft.EntityFrameworkCore;

namespace MEG.Demo.ElasticLogger.Api.DataAccess.DbContext;

public class LibraryDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly IAuditLogger _auditLogger;
    public LibraryDbContext(DbContextOptions options,IAuditLogger auditLogger) : base(options)
    {
        _auditLogger = auditLogger;
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<AuthorEntity> Authors { get; set; }
    

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new ())
    {
        await _auditLogger.AddAsync(ChangeTracker, cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
