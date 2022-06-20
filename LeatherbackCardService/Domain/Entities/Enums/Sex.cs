using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum Sex
    {
        [Description("Male")]
        Male = 1,
        [Description("Female")]
        Female,
    }
}