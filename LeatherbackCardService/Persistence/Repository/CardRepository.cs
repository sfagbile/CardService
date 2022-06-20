using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Cards;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Specification;

namespace Persistence.Repository
{
    public class CardRepository : ICardsRepository
    {
        private readonly ICardServiceDbContext _dbContext;

        public CardRepository(ICardServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Save()
        {
            var result = false;
            try
            {
                result = await _dbContext.SaveChangesAsync() >= 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return result;
        }

        public async Task InsertAsync(Card entity)
        {
            await _dbContext.Cards.AddRangeAsync(entity);
        }

        public async Task InsertManyAsync(IEnumerable<Card> entities)
        {
            await _dbContext.Cards.AddRangeAsync(entities);
        }

        public void DeleteAsync(Card entity)
        {
            entity.IsDeleted = true;
            _dbContext.Cards.Update(entity);
        }

        public void UpdateAsync(Card entity)
        {
            _dbContext.Cards.Update(entity);
        }

        public void UpdateAsync(params Card[] entities)
        {
            _dbContext.Cards.UpdateRange(entities);
        }

        public Task<bool> AnyAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Card> GetFirstOrDefault(Expression<Func<Card, bool>> predicate = null,
            bool disableTracking = true)
        {
            var query = _dbContext.Cards.AsQueryable();
            if (disableTracking) query = query.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Card> GetFirstOrDefault(BaseSpecification<Card> specification, bool disableTracking = true)
        {
            var query = _dbContext.Cards.AsQueryable();
            if (disableTracking) query = query.AsNoTracking();
            query = query.Where(specification.ToExpression());

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Card>> Search(Expression<Func<Card, bool>> predicate = null,
            Func<IQueryable<Card>, IOrderedQueryable<Card>> orderBy = null, int? size = null, int? skip = null,
            bool disableTracking = true)
        {
            var query = _dbContext.Cards.Where(x => !x.IsDeleted);
            if (disableTracking) query = query.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (size.HasValue) query = query.Take(size.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Card>> Search(BaseSpecification<Card> specification, int? size = null,
            int? skip = null, bool disableTracking = true)
        {
            var query = _dbContext.Cards.Where(x => !x.IsDeleted);
            if (disableTracking) query = query.AsNoTracking();
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (size.HasValue) query = query.Take(size.Value);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetAll()
        {
            return await _dbContext.Cards.AsNoTracking().ToListAsync();
        }

        public IQueryable<Card> Query()
        {
            return _dbContext.Cards.AsNoTracking().Where(a => a.IsDeleted == false);
        }
    }
}