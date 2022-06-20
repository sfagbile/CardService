using System;

namespace ApplicationServices.ViewModels.RailsBank
{
    public class GetLedgerByCustomerIdViewModel
    {
        public Guid EndUserId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid LedgerId { get; set; }
    }
}