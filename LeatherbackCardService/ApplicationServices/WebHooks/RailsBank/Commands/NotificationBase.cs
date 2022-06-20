using System;
using Newtonsoft.Json;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class NotificationBase
    {
        public string Owner { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NotificationId { get; set; }
    }
}