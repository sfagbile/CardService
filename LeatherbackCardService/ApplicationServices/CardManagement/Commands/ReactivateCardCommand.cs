using System;
using ApplicationServices.CardManagement.Models;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardManagement.Commands
{
    public class ReactivateCardCommand  : IRequest<Result<CardActivationResponseModel>>
    {
        public Guid CardId { get; set; }
    }
}