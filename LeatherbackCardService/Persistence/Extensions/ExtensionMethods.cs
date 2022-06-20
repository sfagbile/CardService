using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Extensions
{
    public static class ExtensionMethods
    {
        public static void ApplyGlobalFilters<T>(this ModelBuilder modelBuilder, string propertyName, T value)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var foundProperty = entityType.FindProperty(propertyName);
                if (foundProperty != null && foundProperty.ClrType == typeof(T))
                {
                    var newParameter = Expression.Parameter(entityType.ClrType);
                    var filter = Expression.Lambda(Expression.Equal(Expression.Property(newParameter, propertyName),
                        Expression.Constant(value)), newParameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

        }
    }
}