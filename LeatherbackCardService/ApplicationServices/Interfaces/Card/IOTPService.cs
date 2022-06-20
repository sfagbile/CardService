using System.Threading;
using System.Threading.Tasks;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces.Card
{
    public interface IOtpService
    {
        Task<Result<string>> ValidateOtp(string username, string otp,
            CancellationToken cancellationToken);
    }
}