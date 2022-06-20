using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Model.RailsBankModels;
using ApplicationServices.Interfaces.Card;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.BaseResponse;
using Shared.Encryption;
using Shared.Extensions;

namespace ApplicationServices.Card.Processors
{
    public class RailsBankGetCardDetailsProcessor : ICardService
    {
        private readonly AESEncryption _encryptionUtil;
        private readonly IConfiguration _configuration;
        private readonly IMeaWalletService _meaWalletService;
        private readonly IRailsBankService _railsBankService;
        private readonly RSAEncryption _rsaEncryption;

        public string ResolverValue { get; } = nameof(Provider.RailsBank);
        public bool HasWebHook { get; } = false;

        public RailsBankGetCardDetailsProcessor(AESEncryption encryptionUtil, IConfiguration configuration,
            IMeaWalletService meaWalletService, IRailsBankService railsBankService, RSAEncryption rsaEncryption)
        {
            _encryptionUtil = encryptionUtil;
            _configuration = configuration;
            _meaWalletService = meaWalletService;
            _railsBankService = railsBankService;
            _rsaEncryption = rsaEncryption;
        }

        public async Task<Result<GetCardViewModel>> GetCardDetails(GetCardViewModel cardViewModel,
            CancellationToken cancellationToken)
        {
            //return await GetCardDetails(cardViewModel);
            return await GetMockedCardDetails(cardViewModel);
        }

        private async Task<Result<GetCardViewModel>> GetCardDetails(GetCardViewModel cardViewModel)
        {
            var keyAndIv = _encryptionUtil.GenerateKeyAndIV();
            var secret = keyAndIv.Item1;
            var iv = keyAndIv.Item2;
            var meaTraceId = Guid.NewGuid().ToString().ToLower();

            //Warp encryptionKey
            var encryptionKey = _rsaEncryption.Encrypt(secret,
                _configuration["MeaWallet:EncryptionKey"]);

            var meaWalletResult = await _meaWalletService
                .Post<RailsBankCardDetailsCardIdResponseModel, RailsBankError, RailsBankGetTotpRequestModel>(
                    new RailsBankGetTotpRequestModel
                    {
                        Secret = secret, CardId = cardViewModel.CardIdentifier,
                        EncryptionKey = encryptionKey,
                        PublicKeyFingerprint = _configuration["MeaWallet:Fingerprint"]
                    },
                    "getPan", meaTraceId, iv
                );

            if (!meaWalletResult.IsSuccess) return Result.Fail(cardViewModel, meaWalletResult.Value.Item2.Error, "");

            var cardDetailsResponseModel = meaWalletResult.Value.Item1;

            var decryptedJsonData = _encryptionUtil.DecryptText(cardDetailsResponseModel.EncryptedData,
                secret, cardDetailsResponseModel.Iv);

            var encryptedData = JsonConvert.DeserializeObject<EncryptedData>(decryptedJsonData);

            cardViewModel.Cvv = encryptedData?.Cvv;
            cardViewModel.CardNumber = encryptedData?.Pan;
            cardViewModel.ExpireMonth = encryptedData?.Expiry.Split('-')[1];
            cardViewModel.ExpireYear = encryptedData?.Expiry.Split('-')[0];
            cardViewModel.CardHolderName = encryptedData?.EmbossName;

            return Result.Ok(cardViewModel);
        }


        private static async Task<Result<GetCardViewModel>> GetMockedCardDetails(GetCardViewModel cardViewModel)
        {
            Random rnd = new Random();

            cardViewModel.Cvv = rnd.Next(100, 999).ToString();
            cardViewModel.ExpireYear = "27";
            cardViewModel.ExpireMonth = "12";
            var truncatedPan = cardViewModel.MaskedPan?.Split(" ")[3] ?? rnd.Next(1000, 9999).ToString();
            cardViewModel.CardNumber = $"5399{rnd.Next(1000, 9999)}{rnd.Next(1000, 9999)}{truncatedPan}";

            return await Task.FromResult(Result.Ok(cardViewModel));
        }
    }
}