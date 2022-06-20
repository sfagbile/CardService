using System;

namespace ApplicationServices.CardManagement.Models
{
    public class CardActivationRequestModel
    {
        public Guid RequestId { get; set; }
        public Domain.Entities.Cards.Card Card { get; set; }
    }
}