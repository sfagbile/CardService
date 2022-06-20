namespace ApplicationServices.CardIssuance.Model.RailsBank
{
    public class RailsBankPhysicalCardRequestModel
    {
        public string LedgerId { get; set; }
        public string CardCarrierType { get; set; }
        public string CardDeliveryName { get; set; }
        public string CardType { get; set; }
        public string CardDeliveryMethod { get; set; }
        public string CardDesign { get; set; }
        public RailsBankCardDeliveryAddressModel CardDeliveryAddress { get; set; } = new RailsBankCardDeliveryAddressModel();
        public string[] AdditionalLedgers { get; set; }
        public string CardProgramme { get; set; }
        public string Telephone { get; set; }
    }
}