using System;
using Domain.Entities.Enums;

namespace ApplicationServices.TransactionNotification.Models
{
    public class TransactionNotificationResponseModel
    {
        public Guid TransactionId { get; set; }
        public string ProviderResponse { get; set; }
        public TransactionStatus Status { get; set; }
    }
}