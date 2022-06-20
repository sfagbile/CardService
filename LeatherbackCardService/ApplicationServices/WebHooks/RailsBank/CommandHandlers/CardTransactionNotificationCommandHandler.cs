using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces;
using ApplicationServices.WebHooks.RailsBank.Commands;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.CommandHandlers
{
    public class CardTransactionNotificationCommandHandler : IRequestHandler<CardTransactionNotificationCommand, Result>
    {
        private readonly ILogger<CardTransactionNotificationCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly ICardServiceDbContext _dbContext;
        private readonly IMessagePublisher _messagePublisher;

        public CardTransactionNotificationCommandHandler(ILogger<CardTransactionNotificationCommandHandler> logger,
            IMediator mediator, ICardServiceDbContext dbContext, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        public Task<Result> Handle(CardTransactionNotificationCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}