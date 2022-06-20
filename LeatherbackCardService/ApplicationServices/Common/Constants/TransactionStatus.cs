namespace ApplicationServices.Common.Constants
{
    public class TransactionStatus
    {
        public const string Failed = "failed";
        public const string ProcessingStarted = "processing_started";
        public const string Successful = "successful";
        public const string Reversed = "reversed";
        public const string Expired = "expired";
        public const string InstructionsSent = "instructions_sent";
        public const string Approved = "approved";
        public const string New = "new";
        public const string Validated = "validated";
        public const string ApprovalNeeded = "approval_needed";
        public const string ApprovalRequested = "approval_requested";
        public const string Rejected = "rejected";
        public const string Scheduled = "scheduled";
        public const string Processed = "processed";
        public const string ProcessedWithErrors = "processed_with_errors";
        public const string Cancelled = "cancelled";
    }
}