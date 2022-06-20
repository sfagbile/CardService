using System.Collections.Generic;

namespace ApplicationServices.Common.Options
{
    public class RailsBankRoot
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public RailsBankLedgerMeta LedgerMeta { get; set; }
        public string AssetClass { get; set; }
        public string AssetType { get; set; }
        public List<string> Countries { get; set; }
        public string CardProgramme { get; set; }
        public string CardDesign { get; set; }
        public string LedgerPrimaryUseTypes { get; set; }
        public string LedgerWhoOwnsAssets { get; set; }
        public string LedgerTAndCsCountryOfJurisdiction { get; set; }
        public string PartnerProduct { get; set; }
        public string LedgerType { get; set; }
    }

    public class RailsBankLedgerMeta
    {
        public string CustomerId { get; set; }
        public string AccountType { get; set; }
    }
}