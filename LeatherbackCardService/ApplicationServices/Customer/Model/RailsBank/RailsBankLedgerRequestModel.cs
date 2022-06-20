using ApplicationServices.Common.Options;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankLedgerRequestModel
    {
        public string HolderId { get; set; }
        public string AssetClass { get; set; }
        public string AssetType { get; set; }
        public RailsBankLedgerMeta LedgerMeta { get; set; }
        
       // public string Product { get; set; }

        public class Factory
        {
            public static RailsBankLedgerRequestModel CreateInstance(RailsBankRoot root, string holderId, string assetType)
                => new RailsBankLedgerRequestModel()
                {
                    HolderId = holderId,
                    AssetClass = root.AssetClass,
                    AssetType = assetType,
                    LedgerMeta = new RailsBankLedgerMeta()
                    {
                        CustomerId = ""
                    }
                };

        }
    }
}