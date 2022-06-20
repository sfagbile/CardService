using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.Command
{
    public class AddCooperateEndUserCommand: IRequest<Result<string>>
    {
        public Company Company { get; set; } = new Company();
    }
}