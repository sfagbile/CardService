using System;

namespace Shared.InternalBusMessages
{
    public class CardActivationMessage
    {
        public Guid RequestId { get; set; }
        public Guid CardId { get; set; }
    }
}