using System.Threading.Tasks;

namespace ApplicationServices.Interfaces
{
    public interface ITypedHubClient
    {
        Task BroadCastWalletDebitTransactionMessage(string content);
        Task BroadCastWalletCreditTransactionMessage(string content);
        Task BroadCastFailedTransactionsMessage(string message);
        Task JoinedRoomConfirmation(string content);
    }
}