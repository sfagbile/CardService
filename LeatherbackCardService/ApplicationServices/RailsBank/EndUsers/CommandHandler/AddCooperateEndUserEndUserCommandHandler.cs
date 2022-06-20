using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.RailsBank.EndUsers.Command;
using ApplicationServices.ViewModels.RailsBank;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.CommandHandler
{
    public class AddCooperateEndUserEndUserCommandHandler : IRequestHandler<AddCooperateEndUserCommand, Result<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public AddCooperateEndUserEndUserCommandHandler(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(AddCooperateEndUserCommand request,
            CancellationToken cancellationToken)
        {
            var endUser = _mapper.Map<AddCooperateEndUserCommand, CompanyEndUserViewModelRequest>(request);
            var result =
                await _paymentService.Post<CreatedEndUserResponse, RailsBankError, CompanyEndUserViewModelRequest>(
                    endUser, "customer/endusers");
            var (response, error, isSuccessful) = result.Value;
            if (isSuccessful)
            {
                return Result.Ok<string>(response.EndUserId);
            }

            var errorPath = error.Path != null
                ? string.Join(",", error.Path.Select(n => n.ToString()).ToArray())
                : error.Error;

            return Result.Fail<string>($"The following fields are not populated: {errorPath}");
        }
    }
}