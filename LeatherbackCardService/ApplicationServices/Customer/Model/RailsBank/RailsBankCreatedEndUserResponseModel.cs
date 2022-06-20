using Newtonsoft.Json;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankCreatedEndUserResponseModel
    {
        [JsonProperty("enduser_id")]
        public string EndUserId { get; set; }
    }
}