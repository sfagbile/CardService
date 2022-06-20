using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.BaseResponse;

namespace Domain.Interfaces
{
    public interface IHttpService
    {
        Task<Result<(TResult, TError, bool)>> Post<TResult, TError, T>(T model, string url) where T : class;
        Task<(string, TError, bool)> Post<TError>(Dictionary<string, string> param, string url);
        Task<Result<(TResult, TError, bool)>> Get<TResult, TError>(string url);
        Task<Result<(TResult, TError, bool)>> Post<TResult, TError>(Dictionary<string, string> param, string url);
    }
}