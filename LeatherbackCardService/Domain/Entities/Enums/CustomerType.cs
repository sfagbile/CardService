using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum CustomerType
    {
        [Description("Individual")]
        Individual = 1,
        [Description("Company")]
        Company,
    }
}