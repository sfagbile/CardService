using System;
using ApplicationServices.CardManagement.Models;
using MediatR;
using Shared.BaseResponse;

namespace ApplicationServices.CardManagement.Commands
{
    public class DeleteCardsCommand : IRequest<Result<CardClosureResponseModel>>
    {
        public Guid CardId { get; set; }
        public string Reason { get; set; }
        public string UserName { get; set; }
        public string Otp { get; set; }
    }
}