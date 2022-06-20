using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Context;
using Shared;
using Shared.Repository;

namespace Persistence.Repository
{
    public abstract class BaseCommandRepository<T> : ICommandRepository<T> where T : Entity<Guid>
    {
        private readonly CardServiceDbContext _dbContext;

        public BaseCommandRepository(CardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Save()
        {
            return await ((Microsoft.EntityFrameworkCore.DbContext) _dbContext).SaveChangesAsync() >= 0;
        }

        public async Task InsertAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task InsertManyAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
        }

        public void UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void UpdateAsync(params T[] entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}