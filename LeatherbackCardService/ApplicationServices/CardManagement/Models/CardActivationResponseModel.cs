using System;
using Domain.Entities.Enums;

namespace ApplicationServices.CardManagement.Models
{
    public class CardActivationResponseModel
    {
        public Guid CardId { get; set; }
        public string CardActivationStatus { get; set; } 
        public string ProviderResponse { get; set; }
        public RequestStatus Status { get; set; }
    }
}