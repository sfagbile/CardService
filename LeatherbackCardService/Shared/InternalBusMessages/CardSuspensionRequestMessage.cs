using System;

namespace Shared.InternalBusMessages
{
    public class CardSuspensionRequestMessage
    {
        public Guid RequestId { get; set; }
        public Guid CardId { get; set; }
        public string Reason { get; set; }
    }
}