using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Common.Options;
using ApplicationServices.Customer.Model;
using ApplicationServices.Customer.Model.RailsBank;
using ApplicationServices.Interfaces.CardDetailServices;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Cards;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using Microsoft.Extensions.Configuration;
using Shared.BaseResponse;

namespace ApplicationServices.Customer.Processors.RailsBankProcessor
{
    public class RailsBankCardDetailProcessor : ICardDetailService
    {
        private readonly IRailsBankService _railsBankService;
        private readonly IConfiguration _configuration;
        

        public RailsBankCardDetailProcessor(IRailsBankService railsBankService, IConfiguration configuration)
        {
            _railsBankService = railsBankService;
            _configuration = configuration;
        }

        public async Task<Result<CreateLedgerResponseModel>> CreatedVirtualCardDetails(CardDetail cardDetail,
            CancellationToken cancellationToken) => await CreatedVirtualLedger(cardDetail);

        public async Task<Result<CreateLedgerResponseModel>> CreatePhysicalCardDetails(CardDetail cardDetail,
            CancellationToken cancellationToken) => await CreatePhysicalLedger(cardDetail);

        private async Task<Result<CreateLedgerResponseModel>> CreatedVirtualLedger(CardDetail cardDetail)
        {
            var result = await _railsBankService
                .Post<RailsBankCreatedLedgerResponseModel, RailsBankError, RailsBankLedgerRequestModel>(
                    new RailsBankLedgerRequestModel
                    {
                        HolderId = cardDetail.ProviderEndUser.EndUserId,
                        AssetType = cardDetail.Currency.Code,
                        AssetClass = "currency",
                        LedgerMeta = new RailsBankLedgerMeta
                        {
                            CustomerId = cardDetail.ProviderEndUser.CustomerId.ToString(),
                            AccountType = Enum.GetName(cardDetail.ProviderEndUser.Customer.CustomerType),
                        },
                    }, "customer/ledgers/virtual")
                .ConfigureAwait(false);

            if (result is null || result.IsSuccess == false)
                return Result.Fail(
                    new CreateLedgerResponseModel
                    {
                        Message = result?.Message, ProviderResponse = result?.Value.Item2.ToJson()
                    }, result?.Value.Item2.Error, "");

            return Result.Ok(new CreateLedgerResponseModel
            {
                Message = "Successful",
                ProviderResponse = result.Value.Item1.ToJson(),
                LedgerId = result.Value.Item1.LedgerId
            });
        }

        private async Task<Result<CreateLedgerResponseModel>> CreatePhysicalLedger(CardDetail cardDetail)
        {
            var partnerProduct = new List<string>();
            partnerProduct.Add("ledger-primary-use-types-deposit");
            partnerProduct.Add("ledger-primary-use-types-payments");

            var result =
                await _railsBankService
                    .Post<RailsBankCreatedPhysicalLedgerResponseModel, RailsBankError,
                        RailsBankPhysicalLedgerRequestModel>(
                        new RailsBankPhysicalLedgerRequestModel
                        {
                            AssetClass = "asset_class",
                            AssetType = cardDetail.Currency.Code,
                            HolderId = cardDetail.ProviderEndUser.EndUserId,
                            LedgerMeta =
                                new RailsBankLedgerMeta
                                {
                                    CustomerId = cardDetail.ProviderEndUser.CustomerId.ToString(),
                                    AccountType = Enum.GetName(cardDetail.ProviderEndUser.Customer.CustomerType)
                                },
                            LedgerType = "ledger-type-single-user",
                            LedgerWhoOwnsAssets = "ledger-assets-owned-by-me",
                            PartnerProduct = "ExampleBank-EUR-1",
                            LedgerPrimaryUseTypes = partnerProduct,
                            LedgerTAndCsCountryOfJurisdiction = cardDetail.Currency.Code
                        }, "customer/ledgers");

            if (result is null || result.IsSuccess == false)
                return Result.Fail(
                    new CreateLedgerResponseModel
                    {
                        Message = result?.Message, ProviderResponse = result?.Value.Item2.ToJson()
                    }, result?.Value.Item2.Error, "");

            return Result.Ok(new CreateLedgerResponseModel
            {
                Message = "Successful",
                ProviderResponse = result.Value.Item1.ToJson(),
                LedgerId = result.Value.Item1.LedgerId
            });
        }

        public bool HasWebHook { get; } = false;
        public string ResolverValue { get; } = nameof(Provider.RailsBank);
    }
}