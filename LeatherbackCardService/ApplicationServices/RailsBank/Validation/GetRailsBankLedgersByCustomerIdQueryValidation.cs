using ApplicationServices.RailsBank.Ledger.Query;
using FluentValidation;

namespace Leatherback.PaymentGateway.ApplicationServices.RailsBank.Validation
{
    public class GetRailsBankLedgersByCustomerIdQueryValidation: AbstractValidator<GetRailsBankLedgersByCustomerIdQuery>
    {
        public GetRailsBankLedgersByCustomerIdQueryValidation()
        {
            RuleFor(x => x.CustomerId).Must(customerId => customerId != default).WithMessage("Customer id is invalid");
        }
    }
}