using System.Collections.Generic;

namespace ApplicationServices.ViewModels.RailsBank
{
    public class RailsBankError
    {
        public string Error { get; set; }   
        public string Detail { get; set; }
        public string Type { get; set; }
        public List<string> Path { get; set; }
    }
}