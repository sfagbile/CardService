using Newtonsoft.Json;

namespace ApplicationServices.ViewModels.RailsBank
{
    public class RailsBankCardActivatedViewModel
    {
        [JsonProperty("card_id")]
        public string CardId { get; set; }
    }
}