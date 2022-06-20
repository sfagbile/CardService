using System;
using Domain.Entities.Enums;

namespace ApplicationServices.CardManagement.Models
{
    public class CardSuspensionResponseModel
    {
        public Guid CardId { get; set; }
        public string CardSuspensionStatus { get; set; }
        public string ProviderResponse { get; set; }
        public RequestStatus Status { get; set; }
    }
}