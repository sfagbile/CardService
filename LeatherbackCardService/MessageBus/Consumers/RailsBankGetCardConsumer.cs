using System;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Model.RailsBankModels;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.InternalBusMessages;

namespace MessageBus.Consumers
{
    public class RailsBankGetCardConsumer : IConsumer<RailsBankGetCardDetailsMessage>
    {
        private readonly ILogger<RailsBankGetCardConsumer> _logger;
        private readonly IServiceProvider _provider;
        private readonly IRailsBankService _railsBankService;

        public RailsBankGetCardConsumer(ILogger<RailsBankGetCardConsumer> logger, IServiceProvider provider,
            IRailsBankService railsBankService)
        {
            _logger = logger;
            _provider = provider;
            _railsBankService = railsBankService;
        }

        public async Task Consume(ConsumeContext<RailsBankGetCardDetailsMessage> context)
        {
            var message = context.Message;
            using (var scope = _provider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ICardServiceDbContext>();
                var card = await dbContext.Cards.FirstOrDefaultAsync(x =>
                    x.Id == message.CardId);

                var result = await _railsBankService
                    .Get<GetRailsBankCardByIdResponseModel, RailsBankError>(
                        $"/v1/customer/cards/{card.CardIdentifier}");

                if (result.IsSuccess)
                {
                    var resultValue = result.Value;
                    var cardByIdResponseModel = resultValue.Item1;

                    card.CardHolderName = cardByIdResponseModel.NameOnCard;
                    card.ExpireMonth = cardByIdResponseModel.CardExpiryDate.Split('/')[1];
                    card.ExpireYear = cardByIdResponseModel.CardExpiryDate.Split('/')[2];
                    card.ExpiryDate = cardByIdResponseModel.CardExpiryDate;
                    card.CardQrCodeContent = cardByIdResponseModel.QrCodeContent;
                    card.CardToken = cardByIdResponseModel.CardToken;
                    card.ReissuedCardId = cardByIdResponseModel.ReissuedCardId;
                    card.ReissuedCardToken = cardByIdResponseModel.ReissuedCardToken;
                    card.MaskedPan = cardByIdResponseModel.TruncatedPan;

                    dbContext.Cards.Update(card);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}