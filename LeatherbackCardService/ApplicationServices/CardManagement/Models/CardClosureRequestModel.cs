using System;

namespace ApplicationServices.CardManagement.Models
{
    public class CardClosureRequestModel
    {
        public Domain.Entities.Cards.Card Card { get; set; }
        public string Reason { get; set; }
    }
}