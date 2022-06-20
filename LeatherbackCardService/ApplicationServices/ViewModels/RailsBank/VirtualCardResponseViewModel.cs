using System.Text.Json.Serialization;

namespace ApplicationServices.ViewModels.RailsBank
{
    public class VirtualCardResponseViewModel
    {
        [JsonPropertyName("card_id")]
        public string CardId { get; set; }
    }
}