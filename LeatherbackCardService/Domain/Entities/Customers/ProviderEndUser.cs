using System;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Entities.ProviderAggregate;
using Shared;
using Shared.BaseResponse;

namespace Domain.Entities.Customers
{
    public class ProviderEndUser : Entity<Guid>
    {
        private ProviderEndUser(Guid customerId, Guid cardProviderId, Guid cardRequestId)
        {
            CustomerId = customerId;
            CardProviderId = cardProviderId;
            CardRequestId = cardRequestId;
        }

        public static Result<ProviderEndUser> CreateInstance(Guid customerId, string endUserId, Guid cardProviderId, Guid cardRequestId)
        {
            if (customerId == default)
                return Result.Fail<ProviderEndUser>($"{nameof(customerId)} is invalid");

            if (endUserId == default) return Result.Fail<ProviderEndUser>($"{nameof(endUserId)} is invalid");

            if (cardProviderId == default)
                return Result.Fail<ProviderEndUser>($"{nameof(cardProviderId)} is invalid");

           var providerEndUser= new ProviderEndUser(customerId, cardProviderId, cardRequestId);
           providerEndUser.Status = CardRequestStatus.Inprogress;
            return Result.Ok(providerEndUser);
        }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string EndUserId { get; set; }
        public Guid CardProviderId { get; set; }
        public CardProvider CardProvider { get; set; }
        public Guid CardRequestId { get; set; }
        public CardRequest CardRequest { get; set; }
        
        public CardRequestStatus Status { get; set; }
        
        protected override void When(object @event)
        {
            throw new NotImplementedException();
        }
    }
}