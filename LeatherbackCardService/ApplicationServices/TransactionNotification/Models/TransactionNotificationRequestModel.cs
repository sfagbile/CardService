using System;
using Domain.Entities.Cards;

namespace ApplicationServices.TransactionNotification.Models
{
    public class TransactionNotificationRequestModel
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public Domain.Entities.Cards.Card Card { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string Remarks { get; set; }
    }
}