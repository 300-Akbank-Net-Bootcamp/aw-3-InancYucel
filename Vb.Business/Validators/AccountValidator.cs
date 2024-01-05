using System.Text.RegularExpressions;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateAccountValidator : AbstractValidator<AccountRequest>
{
    public CreateAccountValidator() 
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID cannot be empty")
            .WithName("Customer Number");
        
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account Number cannot be empty")
            .WithName("Account Number");
        
        RuleFor(x => x.IBAN)
            .NotEmpty().WithMessage("IBAN cannot be empty")
            .MaximumLength(34).WithMessage("Please enter a valid IBAN number")
            .WithName("IBAN");

        RuleFor(x => x.Balance)
            .NotEmpty().WithMessage("Balance cannot be empty")
            .PrecisionScale(18, 4, false)
            .WithMessage("Amount’ must not be more than 18 digits in total, with allowance for 4 decimals.")
            .WithName("Balance");
        
        RuleFor(x => x.CurrencyType)
            .NotEmpty().WithMessage("Currency Type cannot be empty")
            .MaximumLength(100).WithMessage("Currency Type cannot exceed 100 characters")
            .WithName("CurrencyType");
            
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters")
            .WithName("Name");
    }
}