using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Dto.EntityDto;
using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Validators;

public class CustomerValidator : AbstractValidator<ICustomer>
{
    public CustomerValidator()
    {
        const int bcryptHashSize = 72;
        DateTime firstValidDate = new(2020, 01, 01, 00, 00, 00);

        RuleFor(customer => customer.CustomerId)
            .NotNull()
            .GreaterThan(0)
            .WithName("CustomerId")
            .Unless(customer => customer is CustomerDto);
        RuleFor(customer => customer.Name).NotEmpty().WithName("Name");
        RuleFor(customer => customer.Email).NotEmpty().EmailAddress().WithName("Email");
        RuleFor(customer => customer.Password).Length(bcryptHashSize).WithName("Password");
        RuleFor(customer => customer.RegistrationDate)
            .InclusiveBetween(firstValidDate, DateTime.Now)
            .WithName("RegistrationDate");
    }
}