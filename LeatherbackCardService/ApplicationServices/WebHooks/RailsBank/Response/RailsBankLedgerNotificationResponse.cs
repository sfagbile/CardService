using System;

namespace ApplicationServices.WebHooks.RailsBank.Response
{
    public class RailsBankLedgerNotificationResponse : RailsBankBase
    {
        public string LedgerId { get; set; }
    }
}