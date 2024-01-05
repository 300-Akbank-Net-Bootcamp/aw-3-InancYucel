using System.Text.RegularExpressions;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateAccountTransactionValidator : AbstractValidator<AccountTransactionRequest>
{
    public CreateAccountTransactionValidator() 
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID cannot be empty")
            .WithName("Account Number");
        
        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction Date cannot be empty")
            .Must(BeAValidDate).WithMessage("Invalid date/time") ////Regex Validation for Date Type
            .WithName("Account Number");
        
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount cannot be empty")
            .PrecisionScale(18, 4, false)
            .WithMessage("Amount’ must not be more than 18 digits in total, with allowance for 4 decimals.")
            .WithName("Amount");
        
        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description cannot exceed 300 characters")
            .WithName("Description");
        
        RuleFor(x => x.TransferType)
            .NotEmpty().WithMessage("Transfer Type cannot be empty")
            .MaximumLength(10).WithMessage("Transfer Type cannot exceed 100 characters")
            .WithName("Transfer Type");
        
        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("Reference Number cannot be empty")
            .MaximumLength(50).WithMessage("Transfer Type cannot exceed 50 characters")
            .WithName("Transfer Type");
    }
    
    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}