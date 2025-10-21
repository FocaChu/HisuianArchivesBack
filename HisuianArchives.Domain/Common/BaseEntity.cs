using System.ComponentModel.DataAnnotations.Schema;

namespace HisuianArchives.Domain.Common
{
    public abstract class BaseEntity<TId> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;


        private readonly List<BaseEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
