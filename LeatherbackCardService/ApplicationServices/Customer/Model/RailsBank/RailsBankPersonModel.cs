using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApplicationServices.ViewModels.RailsBank;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankPersonModel
    {
        [JsonPropertyName("full_name")]
        public RailsBankFullNameModel FullName { get; set; } = new RailsBankFullNameModel();
        public string Email { get; set; }
        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public RailsBankAddressModel Address { get; set; }
        public List<RailsBankAddressHistoryModel> AddressHistory { get; set; } = new List<RailsBankAddressHistoryModel>();
        public List<string> Nationality { get; set; } = new List<string>();
        public List<string> CountryOfResidence { get; set; } = new List<string>();
        [JsonPropertyName("date_onboarded")]
        public string DateOnboarded { get; set; }
    }
}