using System.Collections.Generic;
using ApplicationServices.ViewModels.RailsBank;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankCompanyModel
    {
        public string Name { get; set; }
        public string TradingName { get; set; }
        public string WebSite { get; set; }
        public string Industry { get; set; }
        public bool ListedOnStockExchange { get; set; }
        public string RegistrationNumber { get; set; }
        public RailsBankAddressModel RegistrationAddress { get; set; }
        public List<RailsBankDirectorsModel> Directors { get; set; } = new List<RailsBankDirectorsModel>();
    }
}