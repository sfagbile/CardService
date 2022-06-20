using System;
using ApplicationServices.CardManagement.Models;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardManagement.Commands
{
    public class CardSuspensionCommand: IRequest<Result<CardSuspensionResponseModel>>
    {
        public Guid CardId { get; set; }
        public string Reason { get; set; }
       // public string Pin { get; set; }
    }
}