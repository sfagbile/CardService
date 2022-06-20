namespace ApplicationServices.Card.Model.RailsBankModels
{
    public class RailsBankCardDetailsCardIdResponseModel
    {
        public string EncryptedData { get; set; }
        public string Iv { get; set; }
    }
    
    public class EncryptedData
    {
        public string Pan { get; set; }
        public string Cvv { get; set; }
        public string Expiry { get; set; }
        public string EmbossName { get; set; }
    }
}