using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;


public class Customer : ICustomer
{
    public int? CustomerId { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }

    public Customer(
        int? id, 
        string name, 
        string email, 
        string password, 
        DateTime? registrationDate)
    {
        CustomerId = id;
        Name = name;
        Email = email;
        Password = password;
        RegistrationDate = registrationDate ?? DateTime.Now;

        this.Validate();
    }
    
    public static Customer CreateCustomer(CustomerDto dto)
    {
        return new Customer(
            dto.CustomerId, 
            dto.Name, 
            dto.Email, 
            dto.Password, 
            dto.RegistrationDate
        );
    }
}

public static class CustomerExtension {
    private static CustomerValidator Validator { get; } = new();

    public static void Validate(this ICustomer customer) => 
        Validator.ValidateAndThrow(customer);
}
