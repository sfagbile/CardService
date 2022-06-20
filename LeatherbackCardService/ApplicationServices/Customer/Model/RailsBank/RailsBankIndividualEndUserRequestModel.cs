using ApplicationServices.ViewModels.RailsBank;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankIndividualEndUserRequestModel
    {
        public RailsBankPersonModel Person { get; set; }
        public RailsBankEnduserMeta EnduserMeta { get; set; }
    }
}