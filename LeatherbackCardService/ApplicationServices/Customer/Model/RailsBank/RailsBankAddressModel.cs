using System.Text.Json.Serialization;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankAddressModel
    {
        [JsonPropertyName("address_refinement")]
        public string AddressRefinement { get; set; }
        [JsonPropertyName("address_number")]
        public string AddressNumber { get; set; }
        [JsonPropertyName("address_street")]
        public string AddressStreet { get; set; }
        [JsonPropertyName("address_city")]
        public string AddressCity { get; set; }
        [JsonPropertyName("address_postal_code")]
        public string AddressPostalCode { get; set; }
        [JsonPropertyName("address_iso_country")]
        public string AddressIsoCountry { get; set; }
    }
}