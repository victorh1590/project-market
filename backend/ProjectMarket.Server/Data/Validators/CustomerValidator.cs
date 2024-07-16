using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        const int bcryptHashSize = 72;
        DateTime firstValidDate = new(2020, 01, 01, 00, 00, 00);

        RuleFor(customer => customer.CustomerId).GreaterThan(0).WithName("CustomerId");
        RuleFor(customer => customer.Name).NotEmpty().WithName("Name");
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress().WithName("Email");
        RuleFor(customer => customer.Password).Length(bcryptHashSize).WithName("Password");
        RuleFor(customer => customer.RegistrationDate)
            .InclusiveBetween(firstValidDate, DateTime.Now)
            .WithName("RegistrationDate");
    }
}