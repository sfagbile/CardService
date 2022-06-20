using ApplicationServices.RailsBank.Ledger.Command;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class CreatePhysicalLedgerCommandValidation: AbstractValidator<CreatePhysicalLedgerCommand>
    {
        public CreatePhysicalLedgerCommandValidation()
        {
            RuleFor(x => x.HolderId).NotNull().WithMessage("End user id can not be null or empty");
            RuleFor(x => x.CustomerId).Must(customerId => customerId != default)
                .WithMessage("Customer id is invalid");
        }
    }
}