using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Enums;

namespace ApplicationServices.Common.Helpers
{
    public static class Helper
    {

        public static string GetResponseMessage(string code, Dictionary<string, string> messages)
        {
            var message = messages.FirstOrDefault(x => x.Key == code).Value;
            return message;
        }

        public static Dictionary<string, TransactionState> GetTransactionStates()
        {
           return new Dictionary<string, TransactionState>
            {
                {"SUCCES", TransactionState.Successful},
                {"PAYMENT_FAILED", TransactionState.Failed},
                {"CREATED", TransactionState.Pending},
                {"INSUFFICIENT_BALANCE", TransactionState.InsufficientBalance},
                {"SERVICE_UNAVAILABLE", TransactionState.ServiceUnavailable},
                {"ERROR_DOUBLE_PAYEMNT", TransactionState.ErrorDoublePayment},
                {"ERROR_AMOUNT_TOO_HIGH", TransactionState.ErrorAmountTooHigh},
                {"ERROR_AMOUNT_TOO_LOW", TransactionState.ErrorAmountTooLow},
                {"WAITING_CUSTOMER_PAYMENT", TransactionState.WaitingCustomerPayment},
                {"WAITING_CUSTOMER_OTP_CODE", TransactionState.WaitingCustomerOtpCode},
                {"WAITING_CUSTOMER_PAYMENT_AT_OPERATOR_SIDE", TransactionState.WaitingCustomerPaymentAtOperatorSide},
                {"WAITING_CUSTOMER_TO_VALIDATE", TransactionState.WaitingCustomerToValidate},
                {"ABONNEMENT_OR_TRANSACTIONS_EXPIRED", TransactionState.AbonementOrTransactionExpired},
                {"ERROR_CURRENCY_NOTVALID", TransactionState.ErrorCurrencyNotValid},
                {"ERROR_PHONE_NUMBER_NOT_FOUND", TransactionState.ErrorPhoneNumberNotFound},
                {"ERROR_PHONE_NUMBER_NOT_SUPPORTED", TransactionState.ErrorPhoneNumberNotSupported},
                {"ERROR_PHONE_PREFIX_NOT_SUPPORTED", TransactionState.ErrorPhonePrefixNotSupported},
                {"TRANSACTION_CANCEL", TransactionState.TransactionCancel},
                {"ERROR_MOMOPAY_UNAVAILABLE", TransactionState.ErrorMomoPayUnAvailable},
                {"ERROR_FLOOZPAY_UNAVAILABLE", TransactionState.ErrorFloozPayUnAvailable},
                {"ERROR_OMPAY_UNAVAILABLE", TransactionState.ErrorOmPayUnAvailable},
                {"DAILY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.DailyMaxAmountTransactionReached},
                {"DAILY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.DailyMaxNumberTransactionReached},
                {"MONTHLY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.MonthlyMaxAmountTransactionReached},
                {"MONTHLY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.MonthlyMaxNumberTransactionReached},
                {"WEEKLY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.WeeklyMaxAmountTransactionReached},
                {"WEEKLY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.WeeklyMaxNumberTransactionReached},
                {"UNKNOWN_ERROR", TransactionState.UnknownError}
            };
        }
        
        public static Dictionary<string, TransactionState> GetResponsesForCinStates()
        {
           return new Dictionary<string, TransactionState>
            {
                {"SUCCES", TransactionState.Successful},
                {"PAYMENT_FAILED", TransactionState.Failed},
                {"CREATED", TransactionState.Pending},
                {"INSUFFICIENT_BALANCE", TransactionState.InsufficientBalance},
                {"SERVICE_UNAVAILABLE", TransactionState.ServiceUnavailable},
                {"ERROR_DOUBLE_PAYEMNT", TransactionState.ErrorDoublePayment},
                {"ERROR_AMOUNT_TOO_HIGH", TransactionState.ErrorAmountTooHigh},
                {"ERROR_AMOUNT_TOO_LOW", TransactionState.ErrorAmountTooLow},
                {"WAITING_CUSTOMER_PAYMENT", TransactionState.WaitingCustomerPayment},
                {"WAITING_CUSTOMER_OTP_CODE", TransactionState.WaitingCustomerOtpCode},
                {"WAITING_CUSTOMER_PAYMENT_AT_OPERATOR_SIDE", TransactionState.WaitingCustomerPaymentAtOperatorSide},
                {"WAITING_CUSTOMER_TO_VALIDATE", TransactionState.WaitingCustomerToValidate},
                {"ABONNEMENT_OR_TRANSACTIONS_EXPIRED", TransactionState.AbonementOrTransactionExpired},
                {"ERROR_CURRENCY_NOTVALID", TransactionState.ErrorCurrencyNotValid},
                {"ERROR_PHONE_NUMBER_NOT_FOUND", TransactionState.ErrorPhoneNumberNotFound},
                {"ERROR_PHONE_NUMBER_NOT_SUPPORTED", TransactionState.ErrorPhoneNumberNotSupported},
                {"ERROR_PHONE_PREFIX_NOT_SUPPORTED", TransactionState.ErrorPhonePrefixNotSupported},
                {"TRANSACTION_CANCEL", TransactionState.TransactionCancel},
                {"ERROR_MOMOPAY_UNAVAILABLE", TransactionState.ErrorMomoPayUnAvailable},
                {"ERROR_FLOOZPAY_UNAVAILABLE", TransactionState.ErrorFloozPayUnAvailable},
                {"ERROR_OMPAY_UNAVAILABLE", TransactionState.ErrorOmPayUnAvailable},
                {"DAILY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.DailyMaxAmountTransactionReached},
                {"DAILY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.DailyMaxNumberTransactionReached},
                {"MONTHLY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.MonthlyMaxAmountTransactionReached},
                {"MONTHLY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.MonthlyMaxNumberTransactionReached},
                {"WEEKLY_MAX_AMOUNT_TRANSACTION_REACHED", TransactionState.WeeklyMaxAmountTransactionReached},
                {"WEEKLY_MAX_NUMBER_TRANSACTION_REACHED", TransactionState.WeeklyMaxNumberTransactionReached},
                {"UNKNOWN_ERROR", TransactionState.UnknownError}
            };
        }
    }
}