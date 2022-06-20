using System;

namespace Shared.InternalBusMessages
{
    public class CardRequestMessage
    {
        public Guid CardRequestId { get; set; }
        public bool IsCreateCustomerInitiated { get; set; }
        public bool IsCreateCustomerSuccessful { get; set; }
        public bool IsCreateCustomerCardDetailsInitiated { get; set; }
        public bool IsCreateCustomerCardDetailsSuccessful { get; set; }
        
        public bool IsProviderEndUserInitiated { get; set; }
        public bool IsProviderEndUserSuccessful { get; set; }
        public bool IsCreateCardInitiated { get; set; }
        
        public bool IsCreateCardSuccessful { get; set; }
    }
}