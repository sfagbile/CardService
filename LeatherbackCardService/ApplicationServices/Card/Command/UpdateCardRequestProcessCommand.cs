using System;
using Domain.Entities.Enums;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.Command
{
    public class UpdateCardRequestProcessCommand : IRequest<Result>
    {
        public Guid CardRequestId { get; set; }
        public bool IsCreateCustomerInitiated { get; set; }
        public bool IsCreateCustomerSuccessful { get; set; }
        
        public string CreateCustomerResponse { get; set; }
        
        public string CreateCardDetailsResponse { get; set; }
        public string CreateProviderEndUserResponse { get; set; }
        
        public string CreateCardResponse { get; set; }
        
        public bool IsCreateProviderEndUserInitiated { get; set; }
        public bool IsCreateProviderEndUserSuccessful { get; set; }
        
        public bool IsCreateCardDetailsInitiated { get; set; }
        public bool IsCreateCardDetailsSuccessful { get; set; }
        
        public bool IsCreateCardInitiated { get; set; }
        public bool IsCreateCardSuccessful { get; set; }
        public CardRequestStatus Status { get; set; }
        public bool ShouldPublish { get; set; }
    }
}