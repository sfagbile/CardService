namespace ApplicationServices.WebHooks.RailsBank
{
    public static class NotificationType
    {
        //EndUser
        public const string EndUserFirewallFinished = "enduser-firewall-finished"; 
        public const string EntityFwQuarantine = "entity-fw-quarantine";
        public const string EntityFwMissingData = "entity-fw-missing-data";
        public const string EntityReadyToUse = "entity-ready-to-use";
        
        //Beneficiary
        public const string BeneficiaryFirewallFinished = "beneficiary-firewall-finished";
        
        //Card
        public const string CardAwaitingActivation = "card-awaiting-activation";
        public const string CardActivated = "card-activated"; 
        public const string CardFailedToActivate = "card-failed-to-activate";
        public const string CardSuspended = "card-suspended";
        public const string CardFailedToSuspend = "card-failed-to-suspend";
        public const string CardFailedToReplace = "card-failed-to-replace";
        public const string CardFailed = "card-failed";
        public const string CardClosed = "card-closed";
        public const string CardFailedToClose = "card-failed-to-close";
        
        //Transaction
        public const string TransactionPending = "transaction-pending";
        public const string TransactionAccepted = "transaction-accepted";
        public const string TransactionPendingReview = "transaction-pending-review";
        public const string LedgerChanged = "ledger-changed";
        
        //Card Transaction
        public const string CardTransaction = "card-transaction";
        public const string CardTransactionReceive = "card-transaction-receive";
        
        //Payment
        public const string PaymentTokenCreated = "payment-token-created";
        public const string PaymentTokenAwaitingActivation = "payment-token-awaiting-activation";
        public const string PaymentTokenActive = "payment-token-active";
        public const string PaymentTokenFailedToActivate = "payment-token-failed-to-activate";
        public const string PaymentTokenSuspended = "payment-token-suspended";
        public const string PaymentTokenFailedToSuspend = "payment-token-failed-to-suspend";
        public const string PaymentTokenClosed = "payment-token-closed";
        public const string PaymentTokenFailedToClose = "payment-token-failed-to-close";

    }
}