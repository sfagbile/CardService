using System;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.RailsBank.Cards.Command
{
    public class ActivateCardCommand: IRequest<Result<string>>
    {
        public Guid CardId { get; set; }
    }
}