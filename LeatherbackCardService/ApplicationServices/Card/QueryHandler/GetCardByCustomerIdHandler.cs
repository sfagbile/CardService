using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Card.Model;
using ApplicationServices.Card.Query;
using ApplicationServices.Interfaces;
using ApplicationServices.Interfaces.Card;
using AutoMapper;
using Domain.Entities.Cards.Specifications;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.Card.QueryHandler
{
    public class GetCardByCustomerIdHandler : IRequestHandler<GetCardByCustomerId, Result<List<GetCardViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IStrategyResolver<ICardService> _cardStrategyProcessor;
        public GetCardByCustomerIdHandler(IMapper mapper, IStrategyResolver<ICardService> cardStrategyProcessor)
        {
            _mapper = mapper;
            _cardStrategyProcessor = cardStrategyProcessor;
        }

        public async Task<Result<List<GetCardViewModel>>> Handle(GetCardByCustomerId request,
            CancellationToken cancellationToken)
        {
            var cards = await _repository.Search(spec.ToExpression());

            var enumerable = cards as Domain.Entities.Cards.Card[] ?? cards.ToArray();
            if (!enumerable.Any()) throw new EntityNotFoundException("No card is found for customer");
            
            _cardStrategyProcessor.GetService(cards.)
            
            var getCards =
                _mapper.Map<List<Domain.Entities.Cards.Card>, List<GetCardViewModel>>(
                    enumerable.ToList());
            
            
            
            return Result.Ok(getCards);
        }
    }
}