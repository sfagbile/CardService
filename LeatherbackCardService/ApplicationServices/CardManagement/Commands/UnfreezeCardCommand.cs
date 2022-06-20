using System;
using ApplicationServices.CardManagement.Models;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardManagement.Commands
{
    public class UnfreezeCardCommand : IRequest<Result<CardActivationResponseModel>>
    {
        public Guid CardId { get; set; }
        public string Pin { get; set; }
    }
}