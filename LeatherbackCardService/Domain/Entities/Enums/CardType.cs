using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum CardType
    {
        [Description("virtual")]
        Virtual = 1,
        [Description("physical")]
        Physical,
    }
}