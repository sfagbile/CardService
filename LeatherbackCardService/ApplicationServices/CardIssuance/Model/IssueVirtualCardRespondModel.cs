using Domain.Entities.Enums;

namespace ApplicationServices.CardIssuance.Model
{
    public class IssueVirtualCardRespondModel
    {
        public string CardNumber { get; init; }
        public string CardHolderName { get; set; }
        public string CardDesign { get; set; }
        public string CardProviderId { get; set; }
        public string CardProgramme { get; set; }
        public string Cvv { get; init; }
        public string Month { get; init; }
        public string Year { get; init; }
        public bool IsPinIssued { get; set; } = false;
        public string CardIdentifier { get; set; }
        public string CardQrCodeContent { get; set; }
        public CardStatus Status { get; set; } = CardStatus.CardIssuanceInProgress;
        
        public string CardStatusReason { get; set; }
        public CardCarrierType CardCarrierType { get; set; } = CardCarrierType.Standard;
        public string ProviderResponse { get; set; }
        
        public string MaskedPan { get; set; }
        public string ExpiryDate { get; set; }
    }
}