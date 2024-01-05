using System.Text.RegularExpressions;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateEftTransactionValidator : AbstractValidator<EftTransactionRequest>
{
    public CreateEftTransactionValidator() 
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
        
        RuleFor(x => x.SenderAccount)
            .NotEmpty().WithMessage("Sender Account cannot be empty")
            .MaximumLength(50).WithMessage("Sender Account cannot exceed 50 characters")
            .WithName("Sender Account");
        
        RuleFor(x => x.SenderIban)
            .NotEmpty().WithMessage("Sender Iban cannot be empty")
            .MaximumLength(50).WithMessage("Sender Iban cannot exceed 50 characters")
            .WithName("Sender Iban");
        
        RuleFor(x => x.SenderName)
            .NotEmpty().WithMessage("Sender Name cannot be empty")
            .MaximumLength(50).WithMessage("Sender Name cannot exceed 50 characters")
            .WithName("Sender Name");
        
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