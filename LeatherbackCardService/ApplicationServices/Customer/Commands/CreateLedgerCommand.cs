using System;
using ApplicationServices.Customer.Model;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.Commands
{
    public class CreateLedgerCommand : IRequest<Result<CreateCardDetailResponseModel>>
    {
        public Guid CardRequestId { get; set; }
    }
}