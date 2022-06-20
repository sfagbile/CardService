using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardIssuance.Model;
using ApplicationServices.CardIssuance.Model.RailsBank;
using ApplicationServices.Interfaces.Card;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using Shared.BaseResponse;

namespace ApplicationServices.CardIssuance.Processors
{
    public class RailsBankCardIssuerProcessor : ICardIssuanceService
    {
        private readonly IRailsBankService _railsBankService;
        public bool HasWebHook { get; } = false;
        public string ResolverValue { get; } = nameof(Provider.RailsBank);

        public RailsBankCardIssuerProcessor(IRailsBankService railsBankService)
        {
            _railsBankService = railsBankService;
        }

        public async Task<Result<IssueVirtualCardRespondModel>> CreateVirtualCard(CardDetail cardDetail,
            CancellationToken cancellationToken)
        {
            var result =
                await _railsBankService
                    .Post<RailsBankCardRequestResponseModel, RailsBankError, RailsBankPhysicalCardRequestModel>(
                        new RailsBankPhysicalCardRequestModel
                        {
                            LedgerId = cardDetail.ProviderLedgerId,
                            CardDesign = cardDetail.CardDesign,
                            CardProgramme = "Railsbank-Virtual-1", //cardDetail.CardProgramme,
                            CardCarrierType = Enum.GetName(CardCarrierType.Standard)?.ToLower(),
                            CardType = Enum.GetName(CardType.Virtual)?.ToLower(),
                            CardDeliveryName = cardDetail.ProviderEndUser.Customer.FullName,
                            CardDeliveryAddress = new RailsBankCardDeliveryAddressModel
                            {
                                AddressCity = cardDetail.ProviderEndUser.Customer.City,
                                AddressIsoCountry = cardDetail.ProviderEndUser.Customer.Country.Iso,
                                AddressStreet = cardDetail.ProviderEndUser.Customer.Address,
                                AddressPostalCode = cardDetail.ProviderEndUser.Customer.PostalCode,
                                AddressNumber = "",
                                AddressRefinement = "",
                                AddressRegion = "",
                            },
                            // AdditionalLedgers = new[] {cardDetail.ProviderLedgerId},
                            Telephone = cardDetail.ProviderEndUser.Customer.PhoneNumber,
                        },
                        "customer/cards");
           
            Random rnd = new Random();
            return Result.Ok(new IssueVirtualCardRespondModel
            {
                ProviderResponse = result.Value.Item1.ToJson(),
                CardIdentifier = Guid.NewGuid().ToString(),
                MaskedPan = $"**** **** **** {rnd.Next(1000, 9999)}",
                ExpiryDate = "12/27",
            });

            /* if (result is null || result.IsSuccess is false)
                 return Result.Fail(
                     new IssueVirtualCardRespondModel {ProviderResponse = result?.Value.Item2.ToJson()},
                     result?.Value.Item2.Error, "");
             
             return Result.Ok(new IssueVirtualCardRespondModel
             {
                 ProviderResponse = result.Value.Item1.ToJson(),
                 CardIdentifier = result.Value.Item1.CardId,
             });*/
        }

        public async Task<Result<IssueVirtualCardRespondModel>> CreatePhysicalCard(CardDetail cardDetail,
            CancellationToken cancellationToken)
        {
            var result =
                await _railsBankService
                    .Post<RailsBankCardRequestResponseModel, RailsBankError, RailsBankPhysicalCardRequestModel>(
                        new RailsBankPhysicalCardRequestModel
                        {
                            LedgerId = cardDetail.ProviderLedgerId,
                            CardDesign = cardDetail.CardDesign,
                            CardProgramme = cardDetail.CardProgramme,
                            CardCarrierType = nameof(CardCarrierType.Standard),
                            CardType = nameof(CardType.Physical),
                            CardDeliveryName = cardDetail.ProviderEndUser.Customer.FullName,
                            CardDeliveryAddress = new RailsBankCardDeliveryAddressModel
                            {
                                AddressCity = cardDetail.ProviderEndUser.Customer.City,
                                AddressIsoCountry = cardDetail.ProviderEndUser.Customer.Country.Iso,
                                AddressStreet = cardDetail.ProviderEndUser.Customer.Address,
                                AddressPostalCode = cardDetail.ProviderEndUser.Customer.PostalCode,
                            },
                            Telephone = cardDetail.ProviderEndUser.Customer.PhoneNumber,
                            AdditionalLedgers = new[] {cardDetail.ProviderLedgerId},
                        },
                        "customer/cards");

            return Result.Ok(new IssueVirtualCardRespondModel
            {
                ProviderResponse = result.Value.Item1.ToJson(),
                CardIdentifier = Guid.NewGuid().ToString()
            });

            /* if (result is null || result.IsSuccess is false)
                 return Result.Fail(
                     new IssueVirtualCardRespondModel {ProviderResponse = result?.Value.Item2.ToJson()},
                     result?.Value.Item2.Error, "");
 
 
             return Result.Ok(new IssueVirtualCardRespondModel
             {
                 ProviderResponse = result.Value.Item1.ToJson(),
                 CardIdentifier = result.Value.Item1.CardId,
             });*/
        }
    }
}