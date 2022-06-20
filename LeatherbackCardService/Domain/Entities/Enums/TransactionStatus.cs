using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum TransactionStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("InProgress")]
        InProgress,
        [Description("Completed")]
        Completed,
        [Description("Successful")]
        Successful,
        [Description("Failed")]
        Failed,
    }
}