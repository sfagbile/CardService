namespace ApplicationServices.Common.Command
{
    public interface INameEnquiryBaseCommand
    {
        public string Channel { get; set; }
        public string Currency { get; set; } 
        public string NetworkProvider { get; set; } 
        public string PhoneNumber { get; set; } 
    }
}