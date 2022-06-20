using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Customer.Model;
using ApplicationServices.Customer.Model.RailsBank;
using ApplicationServices.Interfaces.CardDetailServices;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Shared.BaseResponse;
using Shared.Extensions;

namespace ApplicationServices.Customer.Processors.RailsBankProcessor
{
    public class RailsBankProviderEndUserProcessor : IProviderEndUserService
    {
        private readonly IRailsBankService _railsBankService;

        public RailsBankProviderEndUserProcessor(IRailsBankService railsBankService)
        {
            _railsBankService = railsBankService;
        }

        public string ResolverValue { get; } = nameof(Provider.RailsBank);
        public bool HasWebHook { get; } = true;

        public async Task<Result<CreateProviderEndUserResponseModel>> CreateIndividualEndUser(
            Domain.Entities.Customers.Customer customer, CancellationToken cancellationToken)
        {
            var result = await CreateIndividualEndUser(customer);
            if (result is null || result.IsSuccess is false)
                return Result.Fail(
                    new CreateProviderEndUserResponseModel
                    {
                        ProviderResponse = result?.Value.Item2.ToJson(), Message = result?.Message
                    }, result?.Message, "");

            return Result.Ok(new CreateProviderEndUserResponseModel
            {
                Message = "Inprogress",
                ProviderResponse = result.Value.Item1.ToJson(),
                EndUserId = result.Value.Item1.EndUserId,
                Status = RequestStatus.Inprogress
            });
        }

        public async Task<Result<CreateProviderEndUserResponseModel>> CreateCompanyEndUser(
            Domain.Entities.Customers.Customer customer, CancellationToken cancellationToken)
        {
            var result = await CreateCompanyEndUser(customer);
            if (result is null || result.IsSuccess is false)
                return Result.Fail(
                    new CreateProviderEndUserResponseModel
                    {
                        ProviderResponse = result?.Value.Item2.ToJson(), Message = result?.Message
                    }, result?.Message, "");

            return Result.Ok(new CreateProviderEndUserResponseModel
            {
                Message = "Inprogress",
                ProviderResponse = result.Value.Item1.ToJson(),
                EndUserId = result.Value.Item1.EndUserId,
                Status = RequestStatus.Inprogress
            });
        }
        
        private async Task<Result<(RailsBankCreatedEndUserResponseModel, RailsBankError, bool)>> CreateCompanyEndUser(
            Domain.Entities.Customers.Customer customer)
        {
            var result =
                await _railsBankService
                    .Post<RailsBankCreatedEndUserResponseModel, RailsBankError, RailsBankCompanyEndUserRequestModel>(
                        new RailsBankCompanyEndUserRequestModel
                        {
                            Company = new RailsBankCompanyModel
                            {
                                Industry = "",
                                Name = customer.FirstName,
                                RegistrationAddress = new RailsBankAddressModel()
                                {
                                    AddressCity = customer.Address, AddressIsoCountry = customer.Country.Iso
                                },
                                TradingName = customer.FirstName
                            }
                        }, "customer/endusers");
            return result;
        }


        private async Task<Result<(RailsBankCreatedEndUserResponseModel, RailsBankError, bool)>>
            CreateIndividualEndUser(Domain.Entities.Customers.Customer customer)
        {
            var result =
                await
                    _railsBankService
                        .Post<RailsBankCreatedEndUserResponseModel, RailsBankError,
                            RailsBankIndividualEndUserRequestModel>(
                            new RailsBankIndividualEndUserRequestModel
                            {
                                Person = new RailsBankPersonModel
                                {
                                    Address =
                                        new RailsBankAddressModel
                                        {
                                            AddressCity = customer.City,
                                            AddressStreet = customer.Address,
                                            AddressIsoCountry = customer.Country.Iso3,
                                            AddressPostalCode = customer.PostalCode,
                                            AddressNumber = "",
                                            AddressRefinement = "",
                                        },
                                    Email = customer.Email,
                                    Nationality = new List<string>(),
                                    Telephone = customer.PhoneNumber,
                                    DateOnboarded = DateTime.Now.ToString("yyyy-MM-dd"),
                                    FullName = new RailsBankFullNameModel
                                    {
                                        FirstName = customer.FirstName,
                                        LastName = customer.LastName,
                                        MiddleName = customer.MiddleName
                                    },
                                    DateOfBirth = customer.DateOfBirth.ToString("yyyy-MM-dd"),
                                }
                            }, "customer/endusers");

            return result;
        }
    }
}