using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared;

namespace Persistence.Extensions
{
    internal static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : Entity<T>
        {
            builder.HasKey(x => x.Id);
            builder.Ignore(x => x.GetChanges());
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}