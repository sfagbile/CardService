using System;
using System.Collections.Generic;
using ApplicationServices.Common.Options;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankPhysicalLedgerRequestModel
    {
        public List<string> LedgerPrimaryUseTypes { get; set; } = new List<string>();
        //public string CreditDetailsId { get; set; }
        public string LedgerWhoOwnsAssets { get; set; }
        public string HolderId { get; set; }
        public string LedgerTAndCsCountryOfJurisdiction { get; set; }
        public string PartnerProduct { get; set; }
        public string AssetType { get; set; }
        public string AssetClass { get; set; }
        public string LedgerType { get; set; }
        public RailsBankLedgerMeta LedgerMeta { get; set; }

        public class Factory
        {
            public static RailsBankPhysicalLedgerRequestModel CreateInstance(string holderId, RailsBankRoot root, Guid customerId)
                => new RailsBankPhysicalLedgerRequestModel()
                {
                    LedgerPrimaryUseTypes = new List<string>()
                    {
                        root.LedgerPrimaryUseTypes
                    },
                    LedgerWhoOwnsAssets = root.LedgerWhoOwnsAssets,
                    HolderId = holderId,
                    LedgerTAndCsCountryOfJurisdiction = root.LedgerTAndCsCountryOfJurisdiction,
                    PartnerProduct = root.PartnerProduct,
                    AssetClass = root.AssetClass,
                    AssetType = root.AssetType,
                    LedgerType = root.LedgerType,
                    LedgerMeta = new RailsBankLedgerMeta()
                    {
                        CustomerId = customerId.ToString()
                    }
                };

        }
    }
}