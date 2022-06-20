using System;
using System.Linq.Expressions;
using Shared.Specification;

namespace Domain.Entities.Cards.Specifications
{
    public class GetCardsByCustomerIdSpecification: BaseSpecification<Card>
    {
        private readonly Guid _customerId;
        public GetCardsByCustomerIdSpecification(Guid customerId)
        {
            _customerId = customerId;
        }
        public override Expression<Func<Card, bool>> ToExpression()
        {
            return cards => cards.CardIdentifier == _customerId.ToString();
        }
    }
}