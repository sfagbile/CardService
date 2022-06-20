using System.Threading.Tasks;
using Shared.BaseResponse;

namespace Domain.Interfaces
{
    public interface IMeaWalletService: IHttpService
    {
        Task<Result<(TResult, TError, bool)>> Post<TResult, TError, T>(T model, string url, string meaTraceId, string iv) where T : class;
        Task<Result<(TResult, TError, bool)>> Get<TResult, TError>(string url, string iv);
    }
}