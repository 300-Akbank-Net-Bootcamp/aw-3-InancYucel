using System.Text.RegularExpressions;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateContactValidator : AbstractValidator<ContactRequest>
{
    public CreateContactValidator() 
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID cannot be empty")
            .WithName("Customer Number");
        
        RuleFor(x => x.ContactType)
            .NotEmpty().WithMessage("Contact Type cannot be empty")
            .MaximumLength(10).WithMessage("Contact Type cannot exceed 10 characters")
            .WithName("Contact Type");
        
        RuleFor(x => x.Information)
            .NotEmpty().WithMessage("Information cannot be empty")
            .MaximumLength(10).WithMessage("Information cannot exceed 10 characters")
            .WithName("Information");
    }
}