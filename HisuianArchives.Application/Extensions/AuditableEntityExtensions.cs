using HisuianArchives.Domain.Common;

namespace HisuianArchives.Application.Extensions
{
    public static class AuditableEntityExtensions
    {
        public static T Touch<T>(this T entity) where T : IAuditableEntity
        {
            entity.UpdatedAt = DateTime.UtcNow;

            return entity; 
        }
    }
}
