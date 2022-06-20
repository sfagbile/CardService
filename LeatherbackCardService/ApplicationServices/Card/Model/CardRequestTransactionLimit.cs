using System;

namespace ApplicationServices.Card.Model
{
    public class CardLimitViewModel
    {
        public Guid CardLimitTypeId { get; set; }
        public string CardLimitType { get; set; }
        public decimal Amount { get; set; }
    }
}