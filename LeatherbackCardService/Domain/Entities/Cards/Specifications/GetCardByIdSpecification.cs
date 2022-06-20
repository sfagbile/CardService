using System;
using System.Linq.Expressions;
using Shared.Specification;

namespace Domain.Entities.Cards.Specifications
{
    public class GetCardByIdSpecification: BaseSpecification<Card>
    {
        private readonly Guid _id;
        public GetCardByIdSpecification(Guid id)
        {
            _id = id;
        }
        public override Expression<Func<Card, bool>> ToExpression()
        {
            return cards => cards.Id == _id;
        }
    }
}