using System;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public class RailsBankGetEndUserModel
    {
        public Guid EndUserId { get; set; }
        public Guid CustomerId { get; set; }
    }
}