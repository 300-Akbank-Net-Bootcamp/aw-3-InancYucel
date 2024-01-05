using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateAddressValidator : AbstractValidator<AddressRequest>
{
    public CreateAddressValidator() 
    {
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("Address line 1 cannot be empty")
            .Length(20, 100).WithMessage("Address 1 range must be between 20 and 100")
            .WithName("Address line 1");
        
        RuleFor(x => x.Address2)
            .MaximumLength(100).WithMessage("2nd address line cannot exceed 100 characters")
            .WithName("Address line 2");
        
        RuleFor(x => x.PostalCode)
            .Length(6, 10).WithMessage("Postcode range must be between 6 and 10")
            .WithName("Zip code or Postal Code");
        
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country cannot be empty")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters")
            .WithName("Country");
        
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City cannot be empty")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .WithName("City");
        
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID cannot be empty")
            .WithName("Customer Number");
    }
}