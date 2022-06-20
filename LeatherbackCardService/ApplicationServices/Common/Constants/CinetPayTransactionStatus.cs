namespace ApplicationServices.Common.Constants
{
    public class CinetPayTransactionStatus
    {
        public const string SUCCESS = "VAL";// Transfer transaction successful
        public const string PENDING_PROCESSING = "NEW"; // Transfer transaction pending processing
        public const string PROCESSING = "REC"; // Transfer transaction still in processing
        public const string REJECTED = "REJ";// Transfer transaction rejected by back office
        public const string AWAITING_CONFIRMATION = "NOS"; // Transfer transaction awaiting back office confirmation
    }
}