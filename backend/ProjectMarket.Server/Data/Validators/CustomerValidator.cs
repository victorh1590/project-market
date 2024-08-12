using System.Text;
using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    private static int BCryptHashSize => 60;
    private DateTime FirstValidDate => new(2020, 01, 01, 00, 00, 00);
    
    public CustomerValidator()
    {

        RuleFor(customer => customer.CustomerId)
            .GreaterThan(0)
            .WithName("CustomerId")
            .Unless(customer => customer.CustomerId == null);
        RuleFor(customer => customer.Name).NotNull().NotEmpty().WithName("Name");
        RuleFor(customer => customer.Email).NotNull().NotEmpty().EmailAddress().WithName("Email");
        RuleFor(customer => customer.Password)
            .NotNull()
            .NotEmpty()
            .Must(arr => arr.Length == 60)
            .WithName("Password");
        // RuleFor(customer => customer.PasswordString)
        //     .NotNull()
        //     .Length(BCryptHashSize)
        //     .Equal(customer => Encoding.UTF8.GetString(customer.Password))
        //     .WithName("PasswordString");
        RuleFor(customer => customer.RegistrationDate)
            .NotNull()
            .InclusiveBetween(FirstValidDate, DateTime.Now)
            .WithName("RegistrationDate");
    }
}