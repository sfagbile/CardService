using System;
using System.Linq.Expressions;

namespace Shared.Specification
{
    public abstract class BaseSpecification<T>
    {
        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }
        public abstract Expression<Func<T, bool>> ToExpression();
    }
}