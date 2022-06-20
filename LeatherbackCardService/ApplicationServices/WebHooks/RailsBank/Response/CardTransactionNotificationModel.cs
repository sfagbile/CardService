using System;
using System.Collections.Generic;

namespace ApplicationServices.WebHooks.RailsBank.Response
{
    public class RailsBankCardTransactionNotificationModel
    {
        public class Address
        {
            public string Country { get; set; }
            public string Lines { get; set; }
        }

        public class InstructedAgent
        {
            public string Bic { get; set; }
        }

        public class InstructingAgent
        {
            public string Bic { get; set; }
        }

        public class PaymentInfo
        {
            public string Foo { get; set; }
        }

        public class PointOfSaleCapability
        {
            public PointOfSaleCapability(List<string> cardholderAuth)
            {
                CardholderAuth = cardholderAuth;
            }

            public string TerminalOutput { get; set; }
            public string TerminalEnvironment { get; set; }
            public List<string> CardDataInput { get; set; }
            public string CardCapture { get; set; }
            public string PartialApprovalSupported { get; set; }
            public string CardDataOutput { get; set; }
            public string PurchaseAmountOnlyApprovalSupported { get; set; }
            public string TerminalType { get; set; }
            public string PinCapture { get; set; }
            public string TerminalAttendance { get; set; }
            public List<string> CardholderAuth { get; set; }
        }

        public class PointOfSaleData
        {
            public PointOfSaleData(string cardPresent)
            {
                CardPresent = cardPresent;
            }

            public string FallbackIndicator { get; set; }
            public string ScaAssessment { get; set; }
            public string SchemeInstantFundingIndicator { get; set; }
            public string ScaTestPossession { get; set; }
            public string ScaTestInherence { get; set; }
            public string CardDataInputMethod { get; set; }
            public string ProcessorInstantFundingIndicator { get; set; }
            public string ScaExemptionIndicator { get; set; }
            public string ScaProcessorExemptionIndicator { get; set; }
            public string FraudIndicator { get; set; }
            public string ScaTestKnowledge { get; set; }
            public List<string> CardholderAuthMethod { get; set; }
            public string CardholderPresent { get; set; }
            public string EncryptionLevel { get; set; }
            public string ScaAcquirerExemptionIndicator { get; set; }
            public List<string> CardholderAuthEntity { get; set; }
            public string AuthMethod3Ds { get; set; }
            public string AuthAmountIndicator3Ds { get; set; }
            public string DeviceFormFactor { get; set; }
            public string CardPresent { get; set; }
        }

        public class Receiver
        {
            public Address Address { get; set; }
            public string Bic { get; set; }
            public string Name { get; set; }
        }

        public class ReceiverAccount
        {
            public string UkAccountNumber { get; set; }
            public string RoutingNumber { get; set; }
            public string Bic { get; set; }
            public string AccountBsb { get; set; }
            public string AccountNumber { get; set; }
            public string UkSortCode { get; set; }
            public string Iban { get; set; }
        }

        public class ReceiverAgent
        {
            public string bic { get; set; }
        }

        public class ReferenceAccount
        {
            public string UkAccountNumber { get; set; }
            public string RoutingNumber { get; set; }
            public string Bic { get; set; }
            public string AccountBsb { get; set; }
            public string AccountNumber { get; set; }
            public string UkSortCode { get; set; }
            public string Iban { get; set; }
        }

        public class Root
        {
            public Root(List<string> rejectionReasons)
            {
                RejectionReasons = rejectionReasons;
            }

            public string SettlementDate { get; set; }
            public string PaymentType { get; set; }
            public string TransactionType { get; set; }
            public string CardCurrency { get; set; }
            public string ReceiptId { get; set; }
            public string DeclineReasonCode { get; set; }
            public string AmountBeneficiaryAccount { get; set; }
            public string PartnerProductFx { get; set; }
            public List<string> CardRulesBreached { get; set; }
            public string PaymentMethod { get; set; }
            public PaymentInfo PaymentInfo { get; set; }
            public string EnduserVerifiedTransaction { get; set; }
            public string TransactionStatus { get; set; }
            public string DeclineReason { get; set; }
            public string MerchantCity { get; set; }
            public string TransactionAuditNumber { get; set; }
            public string ConversionRate { get; set; }
            public string PointOfSaleReference { get; set; }
            public List<string> MissingData { get; set; }
            public string MccDescription { get; set; }
            public string LedgerToId { get; set; }
            public string AcquirerReferenceNumber { get; set; }
            public string CardExpiryDate { get; set; }
            public string MerchantCountry { get; set; }
            public string CardId { get; set; }
            public string ProcessorTimestamp { get; set; }
            public string OriginalTransactionId { get; set; }
            public string FixedSide { get; set; }
            public string MerchantCategoryCode { get; set; }
            public PointOfSaleData PointOfSaleData { get; set; }
            public string PointOfSaleCountryCode { get; set; }
            public string TransactionDirection { get; set; }
            public string CardUsed { get; set; }
            public string AdditionalInfo { get; set; }
            public string EventGroupId { get; set; }
            public string MerchantPostcode { get; set; }
            public string MerchantbankId { get; set; }
            public TransactionInfo TransactionInfo { get; set; }
            public string Reference { get; set; }
            public PointOfSaleCapability PointOfSaleCapability { get; set; }
            public string SwiftServiceLevel { get; set; }
            public string MerchantDetails { get; set; }
            public string Amount { get; set; }
            public string DebitPaymentId { get; set; }
            public List<string> FailureReasons { get; set; }
            public string LedgerFromId { get; set; }
            public string TransactionId { get; set; }
            public string AmountLedgerFrom { get; set; }
            public string CreationReason { get; set; }
            public string CardEntryMethod { get; set; }
            public string Reason { get; set; }
            public DateTime CreatedAt { get; set; }
            public string PartnerProduct { get; set; }
            public string ConversionDate { get; set; }
            public string MerchantRegion { get; set; }
            public string BeneficiaryId { get; set; }
            public TransactionPrintout TransactionPrintout { get; set; }
            public DateTime EventGroupCreatedAt { get; set; }
            public string DailyUniqueRefence { get; set; }
            public string MerchantName { get; set; }
            public string AssetType { get; set; }
            public string AssetClass { get; set; }
            public string SwiftChargeBearer { get; set; }
            public string UnblockingReason { get; set; }
            public To To { get; set; }
            public string TransactionCurrency { get; set; }
            public TransactionMeta TransactionMeta { get; set; }
            public List<string> RejectionReasons { get; set; }
            public string MerchantStreet { get; set; }
            public string MerchantId { get; set; }
            public string PointOfSaleInfo { get; set; }
            public string AmountLocalCurrency { get; set; }
            public string CardTransactionType { get; set; }
            public string PaymentTokenId { get; set; }
            public string BeneficiaryAccountId { get; set; }
            public string CutOffTimeImplementation { get; set; }
        }

        public class Sender
        {
            public string Name { get; set; }
            public Address Address { get; set; }
        }

        public class SenderAccount
        {
            public string UkAccountNumber { get; set; }
            public string RoutingNumber { get; set; }
            public string Bic { get; set; }
            public string AccountBsb { get; set; }
            public string AccountNumber { get; set; }
            public string UkSortCode { get; set; }
            public string Iban { get; set; }
        }

        public class SenderAgent
        {
            public string Bic { get; set; }
        }

        public class To
        {
            public string Number { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string BankCode { get; set; }
        }

        public class TransactionInfo
        {
            public UltimateReceiver UltimateReceiver { get; set; }
            public string SettlementDate { get; set; }
            public DateTime CreationDateTime { get; set; }
            public string EndToEndId { get; set; }
            public InstructedAgent InstructedAgent { get; set; }
            public string PurposeCategory { get; set; }
            public ReceiverAccount ReceiverAccount { get; set; }
            public string Currency { get; set; }
            public string SettlementMethod { get; set; }
            public string ChargeBearer { get; set; }
            public ReferenceAccount ReferenceAccount { get; set; }
            public string ServiceLevel { get; set; }
            public ReceiverAgent ReceiverAgent { get; set; }
            public Receiver Receiver { get; set; }
            public SenderAccount SenderAccount { get; set; }
            public UltimateSender UltimateSender { get; set; }
            public string InstructionId { get; set; }
            public string Amount { get; set; }
            public string TransactionId { get; set; }
            public string RemittanceInformationUnstructured { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime AcceptanceDateTime { get; set; }
            public SenderAgent SenderAgent { get; set; }
            public string MessageSchema { get; set; }
            public Sender Sender { get; set; }
            public InstructingAgent InstructingAgent { get; set; }
            public string Purpose { get; set; }
            public string MessageId { get; set; }
        }

        public class TransactionMeta
        {
            public string Foo { get; set; }
        }

        public class TransactionPrintout
        {
            public string Foo { get; set; }
        }

        public class UltimateReceiver
        {
            public Address Address { get; set; }
            public string Bic { get; set; }
            public string Name { get; set; }
        }

        public class UltimateSender
        {
            public Address Address { get; set; }
            public string Bic { get; set; }
            public string Name { get; set; }
        }
    }
}