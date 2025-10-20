using HisuianArchives.Application.Services;
using HisuianArchives.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HisuianArchives.Infrastructure.Persistence.Interceptors;

/// <summary>
/// EF Core interceptor to automatically populate auditing properties (Created, LastModified, etc.).
/// </summary>
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IIdentityService _identityService;
    private readonly TimeProvider _dateTimeProvider; 

    /// <summary>
    /// Constructor that injects the required dependencies.
    /// </summary>
    /// <param name="identityService">Service to obtain the current user's ID.</param>
    public AuditableEntityInterceptor(IIdentityService identityService)
    {
        _identityService = identityService;
        _dateTimeProvider = TimeProvider.System; // Uses the system clock
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// The main method that finds and updates auditable entities.
    /// </summary>
    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var userId = _identityService.GetUserId();
        var utcNow = _dateTimeProvider.GetUtcNow();

        var entries = context.ChangeTracker.Entries<BaseAuditableEntity<Guid>>();
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(e => e.CreatedBy).CurrentValue = userId;
                entry.Property(e => e.Created).CurrentValue = utcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                Console.WriteLine($"--- INTERCEPTOR: Setting 'LastModifiedBy' to '{userId ?? "NULL"}' and 'LastModified' to '{utcNow}'. ---");
                entry.Property(e => e.LastModifiedBy).CurrentValue = userId;
                entry.Property(e => e.LastModified).CurrentValue = utcNow;
            }

            var createdProp = entry.Property(e => e.Created).CurrentValue;
            var lastModifiedProp = entry.Property(e => e.LastModified).CurrentValue;
            Console.WriteLine($"--- INTERCEPTOR: After setting, Created = '{createdProp}', LastModified = '{lastModifiedProp}'. ---");
        }
    }
}

public static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}