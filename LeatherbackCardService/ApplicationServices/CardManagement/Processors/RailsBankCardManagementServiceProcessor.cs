using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.CardManagement.Models;
using ApplicationServices.CardManagement.Models.RailsBank;
using ApplicationServices.Interfaces.Card;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Enums;
using Domain.Interfaces;
using LeatherbackSharedLibrary.Caching.Extensions;
using Shared.BaseResponse;
using Shared.Utility;

namespace ApplicationServices.CardManagement.Processors
{
    public class RailsBankCardManagementServiceProcessor : ICardManagementService
    {
        
        public bool HasWebHook { get; } = false;
        public string ResolverValue { get; } = nameof(Provider.RailsBank);
        
        private readonly IRailsBankService _railsBankService;

        public RailsBankCardManagementServiceProcessor(IRailsBankService railsBankService)
        {
            _railsBankService = railsBankService;
        }

        public async Task<Result<CardSuspensionResponseModel>> SuspendCard(
            CardSuspensionRequestModel cardSuspensionRequestModel, CancellationToken cancellationToken)
        {
            return Result.Ok(new CardSuspensionResponseModel
            {
                Status = RequestStatus.Completed,
                CardId = cardSuspensionRequestModel.Card.Id,
                ProviderResponse = "",
                CardSuspensionStatus = EnumUtility.GetEnumDescription(CardStatus.CardSuspended)
            });
            
            /*var result =
                await _railsBankService
                    .Post<RailBankCardManagementResponseModel, RailsBankError, RailBankCardManagementRequestModel>(
                        new RailBankCardManagementRequestModel {SuspendReason = cardSuspensionRequestModel.Reason},
                        $"customer/cards/{cardSuspensionRequestModel.Card.CardIdentifier}/suspend");
            if (result.IsSuccess)
            {
               return Result.Ok(new CardSuspensionResponseModel
                {
                    Status = RequestStatus.Inprogress,
                    CardId = cardSuspensionRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardSuspensionStatus = EnumUtility.GetEnumDescription(CardStatus.CardSuspendedInProgress)
                });
               
            }

            return Result.Fail(
                new CardSuspensionResponseModel
                {
                    Status = RequestStatus.Failed,
                    CardId = cardSuspensionRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardSuspensionStatus = EnumUtility.GetEnumDescription(CardStatus.CardFailed)
                }, EnumUtility.GetEnumDescription(CardStatus.CardFailed), ""); */
        }

        public async Task<Result<CardClosureResponseModel>> CloseCard(CardClosureRequestModel cardClosureRequestModel,
            CancellationToken cancellationToken)
        {
            return Result.Ok(new CardClosureResponseModel
            {
                Status = RequestStatus.Completed,
                CardId = cardClosureRequestModel.Card.Id,
                ProviderResponse = "",
                CardClosureStatus = EnumUtility.GetEnumDescription(CardStatus.CardClosed)
            });
            
            /*var result =
                await _railsBankService
                    .Post<RailBankCardManagementResponseModel, RailsBankError, RailBankCardManagementRequestModel>(
                        new RailBankCardManagementRequestModel {SuspendReason = cardClosureRequestModel.Reason},
                        $"customer/cards/{cardClosureRequestModel.Card.CardIdentifier}/close");
            if (result.IsSuccess)
            {
                return Result.Ok(new CardClosureResponseModel
                {
                    Status = RequestStatus.Inprogress,
                    CardId = cardClosureRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardClosureStatus = EnumUtility.GetEnumDescription(CardStatus.CardClosureInProgress)
                });
            }

            return Result.Fail(
                new CardClosureResponseModel
                {
                    Status = RequestStatus.Failed,
                    CardId = cardClosureRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardClosureStatus = EnumUtility.GetEnumDescription(CardStatus.CardFailedToClose)
                }, EnumUtility.GetEnumDescription(CardStatus.CardFailedToClose), ""); */
        }

        public async Task<Result<CardActivationResponseModel>> ActivateCard(
            CardActivationRequestModel cardActivationRequestModel, CancellationToken cancellationToken)
        {
            return Result.Ok(new CardActivationResponseModel
            {
                Status = RequestStatus.Completed,
                CardId = cardActivationRequestModel.Card.Id,
                ProviderResponse = "",
                CardActivationStatus = EnumUtility.GetEnumDescription(CardStatus.CardActivated)
            });
            
           /* var result =
                await _railsBankService
                    .Post<RailBankCardManagementResponseModel, RailsBankError, RailBankCardManagementRequestModel>(
                        new RailBankCardManagementRequestModel { },
                        $"customer/cards/{cardActivationRequestModel.Card.CardIdentifier}/activate");
            if (result.IsSuccess)
            {
                return Result.Ok(new CardActivationResponseModel
                {
                    Status = RequestStatus.Inprogress,
                    CardId = cardActivationRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardActivationStatus = EnumUtility.GetEnumDescription(CardStatus.CardActivationInProgress)
                });
            }

            return Result.Fail(
                new CardActivationResponseModel
                {
                    Status = RequestStatus.Failed,
                    CardId = cardActivationRequestModel.Card.Id,
                    ProviderResponse = result.ToJson(),
                    CardActivationStatus = EnumUtility.GetEnumDescription(CardStatus.CardFailedToActivate)
                }, EnumUtility.GetEnumDescription(CardStatus.CardFailedToActivate), "");
                */
        }

    }
}