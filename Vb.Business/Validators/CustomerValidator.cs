using System.Text.RegularExpressions;
using FluentValidation;
using Vb.Schema;

namespace Vb.Business.Validators;

public class CreateCustomerValidator : AbstractValidator<CustomerRequest>
{
    public CreateCustomerValidator() 
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("You have to enter First Name")
            .MinimumLength(2).WithMessage("First Name length cannot be less than 2 characters")
            .MaximumLength(50).WithMessage("First Name length cannot exceed 50 characters")
            .WithName("First Name");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("You have to enter Last Name")
            .MinimumLength(2).WithMessage("Last Name length cannot be less than 2 characters")
            .MaximumLength(50).WithMessage("Last Name length cannot exceed 50 characters")
            .WithName("Last Name");
               
        RuleFor(x => x.IdentityNumber)
            .NotEmpty().WithMessage("You have to enter Identity Number")
            .Length(11).WithMessage("Identity Number must be 11 Character")
            .Must(IsTrIdentityNumber).WithMessage("Please enter a identity number") //Regex Validation specific to TR ID number
            .WithName("Identity Number");

        RuleFor(s => s.DateOfBirth)
            .Must(BeAValidDate).WithMessage("Invalid date/time"); ////Regex Validation for Date Type
        
        RuleForEach(x => x.Addresses).SetValidator(new CreateAddressValidator());
        RuleForEach(x => x.Contacts).SetValidator(new CreateContactValidator());
        RuleForEach(x => x.Account).SetValidator(new CreateAccountValidator());
    }
    
    private bool IsTrIdentityNumber(string arg)
    {
        Regex regex = new Regex(@"^[1-9]{1}[0-9]{9}[02468]{1}$");
        return regex.IsMatch(arg);
    }
    
    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}