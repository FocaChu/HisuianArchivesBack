using MediatR;
using HisuianArchives.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HisuianArchives.Infrastructure.Persistence.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchEvents(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchEvents(DbContext? context)
    {
        if (context == null) return;

        var entities = context.ChangeTracker
            .Entries<BaseEntity<Guid>>() 
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}