using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum CardStatus
    {
        [Description("CardIssuanceInProgress")] 
        CardIssuanceInProgress = 1,
        [Description("CardActivated")]
        CardActivated,
        [Description("CardSuspended")]
        CardSuspended,
        [Description("CardSuspendedInProgress")]
        CardSuspendedInProgress,
        [Description("CardAwaitingActivation")]
        CardAwaitingActivation,
        [Description("CardFailedToActivate")]
        CardFailedToActivate,
        [Description("CardActivationInProgress")]
        CardActivationInProgress,
        [Description("CardFailedToSuspend")]
        CardFailedToSuspend,
        [Description("CardFailedToReplace")]
        CardFailedToReplace,
        [Description("CardFailed")]
        CardFailed,
        [Description("CardClosed")]
        CardClosed,
        [Description("CardClosureInProgress")]
        CardClosureInProgress,
        [Description("CardFailedToClose")]
        CardFailedToClose,
        [Description("CardNotFound")]
        CardNotFound
    }
}