using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    private static int BCryptHashSize => 72;
    private DateTime FirstValidDate => new(2020, 01, 01, 00, 00, 00);
    
    public CustomerValidator()
    {

        RuleFor(customer => customer.CustomerId ?? 0)
            .SetValidator(new CustomerIdValidator())
            .WithName("CustomerId")
            .Unless(customer => customer.CustomerId == null);
        RuleFor(customer => customer.Name).NotEmpty().WithName("Name");
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress().WithName("Email");
        RuleFor(customer => customer.Password).Length(BCryptHashSize).WithName("Password");
        RuleFor(customer => customer.RegistrationDate)
            .InclusiveBetween(FirstValidDate, DateTime.Now)
            .WithName("RegistrationDate");
    }
}

public class CustomerIdValidator : AbstractValidator<int>
{
    public CustomerIdValidator()
    {
        RuleFor(customerId => customerId)
            .GreaterThan(0)
            .WithName("CustomerId");
    }
}