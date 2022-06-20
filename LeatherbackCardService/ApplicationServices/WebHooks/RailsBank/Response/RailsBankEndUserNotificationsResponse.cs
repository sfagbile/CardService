using System;

namespace ApplicationServices.WebHooks.RailsBank.Response
{
    public class RailsBankEndUserNotificationsResponse : RailsBankBase
    {
        public string EnduserId { get; set; }
        public string EnduserStatus { get; set; }
    }
}