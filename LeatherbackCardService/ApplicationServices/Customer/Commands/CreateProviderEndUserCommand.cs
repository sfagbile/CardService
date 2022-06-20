using System;
using ApplicationServices.Customer.Model;
using Domain.Entities.ProviderAggregate;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.Commands
{
    public class CreateProviderEndUserCommand: IRequest<Result<CreateProviderEndUserResponseModel>>
    {
        public Guid CardRequestId { get; set; }
        public Guid CardProviderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}