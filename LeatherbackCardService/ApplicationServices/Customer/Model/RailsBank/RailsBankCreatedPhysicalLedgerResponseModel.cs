using Newtonsoft.Json;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankCreatedPhysicalLedgerResponseModel
    {
        [JsonProperty("ledger_id")]
        public string LedgerId { get; set; }
    }
}