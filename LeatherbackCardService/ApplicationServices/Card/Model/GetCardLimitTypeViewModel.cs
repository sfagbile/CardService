using System;

namespace ApplicationServices.Card.Model
{
    public class GetCardLimitTypeViewModel 
    {
        public Guid CardLimitTypeId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}