using ApplicationServices.Common.Options;

namespace ApplicationServices.ViewModels.RailsBank
{
    public class VirtualCardRequestViewModel
    {
        public string LedgerId { get; set; }
        public string CardType { get; set; }
        public string CardDesign { get; set; }
        public string CardProgramme { get; set; }
        public class Factory
        {
            public static VirtualCardRequestViewModel CreateInstance(string ledgerId, RailsBankRoot root)
                => new VirtualCardRequestViewModel()
                {
                    LedgerId = ledgerId,
                    CardType = "Virtual",
                    CardDesign = root.CardDesign,
                    CardProgramme = root.CardProgramme
                };

        }
    }
}