using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApplicationServices.Card.Model.RailsBankModels
{
    public class GetRailsBankCardByIdResponseModel
    {
        [JsonProperty("ledger_id")]
        public string LedgerId { get; set; }
        [JsonProperty("card_token")]
        public string CardToken { get; set; }
        [JsonProperty("name_on_card")]
        public string NameOnCard { get; set; }
        [JsonProperty("card_carrier_type")]
        public string CardCarrierType { get; set; }
        [JsonProperty("card_expiry_date")]
        public string CardExpiryDate { get; set; }
        [JsonProperty("card_id")]
        public string CardId { get; set; }
        [JsonProperty("card_delivery_name")]
        public string CardDeliveryName { get; set; }
        [JsonProperty("card_type")]
        public string CardType { get; set; }
        [JsonProperty("card_rules")]
        public List<RailsBankCardRulesViewModel> CardRules { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("partner_product")]
        public string PartnerProduct { get; set; }
        [JsonProperty("card_delivery_method")]
        public string CardDeliveryMethod { get; set; }
        [JsonProperty("card_design")]
        public string CardDesign { get; set; }
        [JsonProperty("truncated_pan")]
        public string TruncatedPan { get; set; }
        [JsonProperty("card_token")]
        public RailsBankcardDeliveryAddress RailsBankcardDeliveryAddress { get; set; }
        [JsonProperty("card_status")]
        public string CardStatus { get; set; }
        [JsonProperty("card_programme")]
        public string CardProgramme { get; set; }
        
        [JsonProperty("qr_code_content")]
        public string QrCodeContent { get; set; }
        
        [JsonProperty("reissued_card_id")]
        public string ReissuedCardId { get; set; }
        
        [JsonProperty("reissued_card_token")]
        public string ReissuedCardToken { get; set; }
    }

    public class RailsBankCardRulesViewModel
    {
        [JsonProperty("card_rule_type")]
        public string CardRuleType { get; set; }
        [JsonProperty("card_rule_id")]
        public string CardRuleId { get; set; }
        [JsonProperty("card_rule_description")]
        public string CardRuleDescription { get; set; }
        [JsonProperty("card_rule_name")]
        public string CardRuleName { get; set; }
        [JsonProperty("card_rule_body")]
        public string CardRuleBody { get; set; }
    }

    public class RailsBankcardDeliveryAddress
    {
        [JsonProperty("address_iso_country")]
        public string AddressIsoCountry { get; set; }
        [JsonProperty("address_city")]
        public string AddressCity { get; set; } 
        [JsonProperty("address_postal_code")]
        public string AddressPostalCode { get; set; } 
        [JsonProperty("address_street")]
        public string AddressStreet { get; set; } 
        [JsonProperty("address_number")]
        public string AddressNumber { get; set; } 
        [JsonProperty("address_refinement")]
        public string AddressRefinement { get; set; }   
    }
}