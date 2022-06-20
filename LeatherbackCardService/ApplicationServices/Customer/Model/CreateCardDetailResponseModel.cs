using System;
using Domain.Entities.Enums;

namespace ApplicationServices.Customer.Model
{
    public class CreateCardDetailResponseModel
    {
        public Guid CardDetailId { get; set; }
        public string ProviderResponse { get; set; }
        public string Message { get; set; }
        public RequestStatus Status { get; set; }
    }
}