using System;
using Domain.Entities.Enums;

namespace ApplicationServices.CardManagement.Models
{
    public class CardClosureResponseModel
    {
        public Guid CardId { get; set; }
        public string CardClosureStatus { get; set; }
        public string ProviderResponse { get; set; }
        public RequestStatus Status { get; set; }
    }
}