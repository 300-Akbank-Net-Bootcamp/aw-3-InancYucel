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
            .Must(IsTrIbanStandart).WithMessage("Please enter a valid IBAN number")
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
    
    private bool IsTrIbanStandart(string arg)
    {
        Regex regex = new Regex(@"/TR[a-zA-Z0-9]{2}\s?([0-9]{4}\s?){1}([0-9]{1})([a-zA-Z0-9]{3}\s?)([a-zA-Z0-9]{4}\s?){3}([a-zA-Z0-9]{2})\s?/");
        return regex.IsMatch(arg);
    }
}