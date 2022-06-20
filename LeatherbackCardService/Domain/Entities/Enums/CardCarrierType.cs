using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum CardCarrierType
    {
        [Description("renewal")]
        Renewal = 1,
        [Description("replacement")]
        Replacement,
        [Description("standard")]
        Standard,
    }
}