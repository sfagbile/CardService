namespace ApplicationServices.Customer.Model.RailsBank
{
    public  class RailsBankDirectorsModel
    {
        public string DateAppointed { get; set; }
        public string JobTitle { get; set; }
        public bool IsAlsoUbo { get; set; }
        public RailsBankCompanyPersonModel Person { get; set; }
    }
}