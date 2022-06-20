using System;

namespace ApplicationServices.WebHooks.RailsBank.Response
{
    public class RailsBankBase
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NotificationId { get; set; }
        public string Secret { get; set; }
    }
}