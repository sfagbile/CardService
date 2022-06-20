namespace ApplicationServices.WebHooks.RailsBank.Response
{
    public class RailsBankBeneficiaryNotificationResponse : RailsBankBase
    {
        public string BeneficiaryId { get; set; }
        public string BeneficiaryStatus { get; set; }
    }
}