using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Repository
{
    public interface ICommandRepository<in T> where T : class
    {
        Task<bool> Save();
        Task InsertAsync(T entity);
        Task InsertManyAsync(IEnumerable<T> entities);
        void DeleteAsync(T entity);
        void UpdateAsync(T entity);
        void UpdateAsync(params T[] entities);
        Task<bool> AnyAsync(Guid id);
    }
}