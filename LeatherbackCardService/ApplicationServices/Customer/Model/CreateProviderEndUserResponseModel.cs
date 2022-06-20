using System;
using Domain.Entities.Enums;

namespace ApplicationServices.Customer.Model
{
    public class CreateProviderEndUserResponseModel
    {
        public string EndUserId { get; set; }
        public string ProviderResponse { get; set; }
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
    }
}