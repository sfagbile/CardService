using System;
using System.Text.Json.Serialization;
using MediatR;
using Newtonsoft.Json;
using Shared.BaseResponse;

namespace ApplicationServices.WebHooks.RailsBank.Commands
{
    public class RailsBankWebHookCommand : NotificationBase, IRequest<Result>
    {
        
        public string Secret { get; set; }
    
        public string BeneficiaryId { get; set; }
       
        public string BeneficiaryStatus { get; set; }

        public string CardId { get; set; }
        
        public string PaymentTokenId { get; set; }
        
        public string EnduserId { get; set; }
        
        public string EnduserStatus { get; set; }
        public string LedgerId { get; set; }
        public string TransactionId { get; set; }
    }
}