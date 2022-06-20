using System.Collections.Generic;

namespace ApplicationServices.Customer.Model.RailsBank
{
    public  class RailsBankCompanyPersonModel
    {
        public List<string> CountryOfResidence { get; set; } = new List<string>();
        public bool Pep { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Telephone { get; set; }
        public string DateOfBirth { get; set; }
        public List<string> Nationality { get; set; } = new List<string>();
    }
}