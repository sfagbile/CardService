using Newtonsoft.Json;

namespace ApplicationServices.CardIssuance.Model.RailsBank
{
    public class RailsBankCardRequestResponseModel
    {
        [JsonProperty("card_id")]
        public string CardId { get; set; }
    }
}