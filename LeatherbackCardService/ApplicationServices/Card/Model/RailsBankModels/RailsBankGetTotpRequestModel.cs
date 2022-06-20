namespace ApplicationServices.Card.Model.RailsBankModels
{
    public class RailsBankGetTotpRequestModel
    {
        public string CardId { get; set; }
        public string Secret { get; set; }
        public string EncryptionKey { get; set; }
        public string PublicKeyFingerprint { get; set; }
    }
}