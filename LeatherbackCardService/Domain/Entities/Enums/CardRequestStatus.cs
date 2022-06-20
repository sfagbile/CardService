using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum CardRequestStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Inprogress")]
        Inprogress,
        [Description("Completed")]
        Completed,
        [Description("Failed")]
        Failed,
        [Description("Rejected")]
        Rejected,
    }
}