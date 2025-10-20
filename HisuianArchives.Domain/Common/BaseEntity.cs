namespace HisuianArchives.Domain.Common
{
    public abstract class BaseEntity<TId> where TId : notnull
    {
        public TId Id { get; protected set; } = default!;
    }
}
