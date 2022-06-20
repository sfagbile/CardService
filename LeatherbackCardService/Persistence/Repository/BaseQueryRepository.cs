using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Shared;
using Shared.Repository;
using Shared.Specification;

namespace Persistence.Repository
{
    public abstract class BaseQueryRepository<T> : IQueryRepository<T> where T : Entity<Guid>
    {
        private readonly CardServiceDbContext _dbContext;

        public BaseQueryRepository(CardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate = null, bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (disableTracking) query = query.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefault(BaseSpecification<T> specification, bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (disableTracking) query = query.AsNoTracking();
            query = query.Where(specification.ToExpression());

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? size = null, int? skip = null,
            bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().Where(x => !x.IsDeleted);

            if (disableTracking) query = query.AsNoTracking();

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            if (skip.HasValue) query = query.Skip(skip.Value);

            if (size.HasValue) query = query.Take(size.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> Search(BaseSpecification<T> specification, int? size = null, int? skip = null,
            bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().Where(x => !x.IsDeleted);
            if (disableTracking) query = query.AsNoTracking();
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (size.HasValue) query = query.Take(size.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>().AsNoTracking().Where(a => a.IsDeleted == false);
        }
    }
}