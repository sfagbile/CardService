using System;

namespace Leatherback.PaymentGateway.Domain.Events
{
    public static class TransactionsEvent
    {
        public class CustomerExternalAccountDebitedEvent
        {
            public string Email { get; set; }
        }
        public class CustomerLeatherbackAccountDebitedEvent
        {
            public string Email { get; set; }
        }
        
        
        public class CustomerCardDebitedEvent
        {
            public Guid CustomerId { get; set; }
            public decimal Amount { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
        }
    }
}