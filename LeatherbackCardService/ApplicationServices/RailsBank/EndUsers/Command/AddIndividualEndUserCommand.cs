using System;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.Command
{
    public class AddIndividualEndUserCommand: IRequest<Result<string>>
    {
        public Person Person { get; set; } = new Person();
        public EnduserMeta EndUserMeta { get; set; } = new EnduserMeta();
        public Guid CustomerId { get; set; }
    }
}