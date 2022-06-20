using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Shared.Specification;

namespace Shared.Repository
{
    public interface IQueryRepository<T> where T : class
    {
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate = null, bool disableTracking = true);
        Task<T> GetFirstOrDefault(BaseSpecification<T> specification, bool disableTracking = true);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? size = null, int? skip = null, bool disableTracking = true);
        Task<IEnumerable<T>> Search(BaseSpecification<T> specification, int? size = null, int? skip = null, bool disableTracking = true);
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> Query();
    }
}