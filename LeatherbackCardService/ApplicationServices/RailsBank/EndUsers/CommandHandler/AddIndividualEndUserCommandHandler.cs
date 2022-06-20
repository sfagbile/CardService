using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.RailsBank.EndUsers.Command;
using ApplicationServices.ViewModels.RailsBank;
using AutoMapper;
using Domain.Entities.Enums;
using Domain.Entities.RailsBank;
using Domain.Interfaces;
using Leatherback.PaymentGateway.ApplicationServices.ViewModels.RailsBank;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.EndUsers.CommandHandler
{
    public class AddIndividualEndUserCommandHandler : IRequestHandler<AddIndividualEndUserCommand, Result<string>>
    {
        private readonly IRailsBankService _railsBankService;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _railsBankEndUserRepository;

        public AddIndividualEndUserCommandHandler(IRailsBankService railsBankService, IMapper mapper,
            ICustomerRepository railsBankEndUserRepository)
        {
            _railsBankService = railsBankService;
            _mapper = mapper;
            _railsBankEndUserRepository = railsBankEndUserRepository;
        }

        public async Task<Result<string>> Handle(AddIndividualEndUserCommand request,
            CancellationToken cancellationToken)
        {
            var endUser = _mapper.Map<AddIndividualEndUserCommand, IndividualEndUserViewModelRequest>(request);

            var result =
                await _railsBankService.Post<CreatedEndUserResponse, RailsBankError, IndividualEndUserViewModelRequest>(
                    endUser, "customer/endusers");

            var (response, error, isSuccessful) = result.Value;

            if (isSuccessful)
            {
                var isEndUserSaved = await _railsBankEndUserRepository.Save();

                return isEndUserSaved
                    ? Result.Ok<string>(response.EndUserId)
                    : Result.Fail<string>(string.Join(",", error.Path.Select(n => n.ToString()).ToArray()));
            }

            var errorPath = error.Path != null
                ? string.Join(",", result.Value.Item2.Path.Select(n => n.ToString()).ToArray())
                : error.Error;

            return Result.Fail<string>($"The following fields are not populated: {errorPath}"); //invalid-iso-country
        }
    }
}