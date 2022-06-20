using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Interfaces.Card;
using ApplicationServices.ViewModels.OTPService;
using ApplicationServices.ViewModels.RailsBank;
using Domain.Interfaces;
using Shared.BaseResponse;

namespace ApplicationServices.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpHttpService _otpHttpService;

        public OtpService(IOtpHttpService otpHttpService)
        {
            _otpHttpService = otpHttpService;
        }

        public async Task<Result<string>> ValidateOtp(string username, string otp, CancellationToken cancellationToken)
        {
            
            var result = await _otpHttpService
                .Post<OtpResponseViewModel, RailsBankError, OtpRequestViewModel>(new OtpRequestViewModel
                {
                    Otp = otp,
                    Username = username,
                }, "validate-otp");

            if (!result.IsSuccess) return Result.Fail(result.Value.Item2.Error, "");

            var cardDetailsResponseModel = result.Value.Item1;
            
            return Result.Ok<string>("Successful");
        }
    }
}