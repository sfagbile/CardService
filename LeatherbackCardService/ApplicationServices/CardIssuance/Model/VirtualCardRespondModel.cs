using System;
using Domain.Entities.Enums;

namespace ApplicationServices.CardIssuance.Model
{
    public class VirtualCardRespondModel
    {
        public Guid CustomerCardDetailId { get; set; }
        public string CardId { get; set; }
        public string ProviderResponse { get; set; }
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
    }
}